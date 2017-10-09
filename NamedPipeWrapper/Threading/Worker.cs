using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NamedPipeWrapper.Threading
{
    internal delegate void WorkerSucceededEventHandler();
    internal delegate void WorkerExceptionEventHandler(Exception exception);

    class Worker
    {
        private readonly TaskScheduler _callbackThread;

        private static TaskScheduler CurrentTaskScheduler
        {
            get
            {
                return (SynchronizationContext.Current != null
                            ? TaskScheduler.FromCurrentSynchronizationContext()
                            : TaskScheduler.Default);
            }
        }

        public event WorkerSucceededEventHandler Succeeded;
        public event WorkerExceptionEventHandler Error;

        public Worker() : this(CurrentTaskScheduler)
        {
        }

        public Worker(TaskScheduler callbackThread)
        {
            _callbackThread = callbackThread;
        }

        public void DoWork(Action action)
        {
            new Task(DoWorkImpl, action, CancellationToken.None, TaskCreationOptions.LongRunning).Start();
        }

        private void DoWorkImpl(object oAction)
        {
            var action = (Action)oAction;
            try
            {
                action();
                Callback(Succeed);
            }
            catch (Exception e)
            {
                Callback(() => Fail(e));
            }
        }

        private void Succeed() => Succeeded?.Invoke();


        private void Fail(Exception exception) => Error?.Invoke(exception);


        private void Callback(Action action)
        {
            Task.Factory.StartNew(action, CancellationToken.None, TaskCreationOptions.None, _callbackThread);
        }
    }

    
}
