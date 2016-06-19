using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TestTop.Core;

namespace TestTop.Server
{
    public class DesktopModule : NancyModule
    {
        public DesktopModule() : base("/desktops")
        {
            Get["/", true] = GetDesktopList;
            Post["/{intPtr:int}", true] = AddDesktop;
            Put["/{intPtr:int}", true] = UpdateDesktop;
        }

        private Task<dynamic> UpdateDesktop(dynamic ctx, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private Task<dynamic> AddDesktop(dynamic ctx, CancellationToken token)
        {
            throw new NotImplementedException();
        }

        private async Task<dynamic> GetDesktopList(dynamic ctx, CancellationToken token) => new List<Desktop>().ToJson();
    }
}
