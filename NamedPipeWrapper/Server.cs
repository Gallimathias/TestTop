using NamedPipeWrapper.IO;
using NamedPipeWrapper.Threading;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Pipes;

namespace NamedPipeWrapper
{
    /// <summary>
    /// Wraps a <see cref="NamedPipeServerStream"/> and provides multiple simultaneous client connection handling.
    /// </summary>
    /// <typeparam name="TRead">Reference type to read from the named pipe</typeparam>
    /// <typeparam name="TWrite">Reference type to write to the named pipe</typeparam>
    public class Server<TRead, TWrite>
        where TRead : class
        where TWrite : class
    {
        /// <summary>
        /// Invoked whenever a client connects to the server.
        /// </summary>
        public event ConnectionEventHandler<TRead, TWrite> ClientConnected;

        /// <summary>
        /// Invoked whenever a client disconnects from the server.
        /// </summary>
        public event ConnectionEventHandler<TRead, TWrite> ClientDisconnected;

        /// <summary>
        /// Invoked whenever a client sends a message to the server.
        /// </summary>
        public event ConnectionMessageEventHandler<TRead, TWrite> ClientMessage;

        /// <summary>
        /// Invoked whenever an exception is thrown during a read or write operation.
        /// </summary>
        public event PipeExceptionEventHandler Error;

        private readonly string pipeName;
        private readonly PipeSecurity pipeSecurity;
        private readonly ConcurrentDictionary<int, NamedPipeConnection<TRead, TWrite>> connections;

        private int nextPipeId;

        private volatile bool shouldKeepRunning;
        private volatile bool isRunning;

        /// <summary>
        /// Constructs a new <c>NamedPipeServer</c> object that listens for client connections on the given <paramref name="pipeName"/>.
        /// </summary>
        /// <param name="pipeName">Name of the pipe to listen on</param>
        public Server(string pipeName, PipeSecurity pipeSecurity)
        {
            connections = new ConcurrentDictionary<int, NamedPipeConnection<TRead, TWrite>>();
            this.pipeName = pipeName;
            this.pipeSecurity = pipeSecurity;
        }

        /// <summary>
        /// Begins listening for client connections in a separate background thread.
        /// This method returns immediately.
        /// </summary>
        public void Start()
        {
            shouldKeepRunning = true;
            var worker = new Worker();
            worker.Error += OnError;
            worker.DoWork(ListenSync);
        }

        /// <summary>
        /// Sends a message to all connected clients asynchronously.
        /// This method returns immediately, possibly before the message has been sent to all clients.
        /// </summary>
        /// <param name="message"></param>
        public void PushMessage(TWrite message)
        {
            foreach (var client in connections.Values)
            {
                client.PushMessage(message);
            }
        }

        /// <summary>
        /// push message to the given client.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="clientName"></param>
        public void PushMessage(TWrite message, string clientName)
        {
            foreach (var client in connections.Values)
            {
                if (client.Name == clientName)
                    client.PushMessage(message);
            }
        }

        /// <summary>
        /// Closes all open client connections and stops listening for new ones.
        /// </summary>
        public void Stop()
        {
            shouldKeepRunning = false;

            foreach (var client in connections.Values)
            {
                client.Close();
            }

            // If background thread is still listening for a client to connect,
            // initiate a dummy connection that will allow the thread to exit.
            //dummy connection will use the local server name.
            var dummyClient = new NamedPipeClient<TRead, TWrite>(pipeName, ".");
            dummyClient.Start();
            dummyClient.WaitForConnection(TimeSpan.FromSeconds(2));
            dummyClient.Stop();
            dummyClient.WaitForDisconnection(TimeSpan.FromSeconds(2));
        }

        #region Private methods

        private void ListenSync()
        {
            isRunning = true;
            while (shouldKeepRunning)
            {
                WaitForConnection(pipeName, pipeSecurity);
            }
            isRunning = false;
        }

        private void WaitForConnection(string pipeName, PipeSecurity pipeSecurity)
        {
            NamedPipeServerStream handshakePipe = null;
            NamedPipeServerStream dataPipe = null;
            NamedPipeConnection<TRead, TWrite> connection = null;

            var connectionPipeName = GetNextConnectionPipeName(pipeName);

            try
            {
                // Send the client the name of the data pipe to use
                handshakePipe = PipeServerFactory.CreateAndConnectPipe(pipeName, pipeSecurity);
                var handshakeWrapper = new PipeStreamWrapper<string, string>(handshakePipe);
                handshakeWrapper.WriteObject(connectionPipeName);
                handshakeWrapper.WaitForPipeDrain();
                handshakeWrapper.Close();

                // Wait for the client to connect to the data pipe
                dataPipe = PipeServerFactory.CreatePipe(connectionPipeName, pipeSecurity);
                dataPipe.WaitForConnection();

                // Add the client's connection to the list of connections
                connection = ConnectionFactory.CreateConnection<TRead, TWrite>(dataPipe);
                connection.ReceiveMessage += ClientOnReceiveMessage;
                connection.Disconnected += ClientOnDisconnected;
                connection.Error += ConnectionOnError;
                connection.Open();

                connections.TryAdd(connection.Id, connection);


                ClientOnConnected(connection);
            }
            // Catch the IOException that is raised if the pipe is broken or disconnected.
            catch (Exception e)
            {
                Console.Error.WriteLine("Named pipe is broken or disconnected: {0}", e);

                Cleanup(handshakePipe);
                Cleanup(dataPipe);

                ClientOnDisconnected(connection);
            }
        }

        private void ClientOnConnected(NamedPipeConnection<TRead, TWrite> connection)
            => ClientConnected?.Invoke(connection);


        private void ClientOnReceiveMessage(NamedPipeConnection<TRead, TWrite> connection, TRead message)
            => ClientMessage?.Invoke(connection, message);


        private void ClientOnDisconnected(NamedPipeConnection<TRead, TWrite> connection)
        {
            if (connection == null)
                return;

            connections.TryRemove(connection.Id, out NamedPipeConnection<TRead, TWrite> tmpCon);

            ClientDisconnected?.Invoke(tmpCon);
        }

        /// <summary>
        ///     Invoked on the UI thread.
        /// </summary>
        private void ConnectionOnError(NamedPipeConnection<TRead, TWrite> connection, Exception exception)
            => OnError(exception);


        /// <summary>
        ///     Invoked on the UI thread.
        /// </summary>
        /// <param name="exception"></param>
        private void OnError(Exception exception)
            => Error?.Invoke(exception);


        private string GetNextConnectionPipeName(string pipeName)
            => $"{pipeName}_{++nextPipeId}";

        private static void Cleanup(NamedPipeServerStream pipe)
        {
            pipe?.Close();
            pipe?.Dispose();
        }

        #endregion
    }
}
