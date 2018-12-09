using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace Kelson.AspNetCore.AwaitRequests
{
    public class Server
    {
        private readonly Func<Task>[] tasks;

        public Server(string url, params Func<Task>[] tasks)
        {
            this.tasks = tasks;
            WebHost.Start(url, HandleRequests);
        }

        public async Task RunAsync()
        {
            var paths = tasks.Select(t => t()).ToArray();
            foreach (var t in paths)
                await t;
        }

        internal static readonly ConcurrentDictionary<string, RouteEnumerator> RouteObjects
            = new ConcurrentDictionary<string, RouteEnumerator>();

        internal async Task HandleRequests(HttpContext context)
        {
            var item = new ContextItem(context);
            foreach (var routepath in RouteObjects)
            {
                if (routepath.Key.EndsWith(context.Request.Path.Value))
                {
                    routepath.Value.PushRequest(item);
                    item.Handled.WaitOne();
                    break;
                }
            }
        }        
    }
}
