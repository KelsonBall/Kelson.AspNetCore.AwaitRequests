using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Kelson.AspNetCore.AwaitRequests
{
    internal class RouteEnumerator : IAsyncEnumerator<HttpContext>
    {
        private readonly AutoResetEvent requestAdded = new AutoResetEvent(false);

        private ContextItem _current;
        public HttpContext Current { get => _current?.Context; }

        public readonly string Route;

        private readonly ConcurrentQueue<ContextItem> queue
            = new ConcurrentQueue<ContextItem>();

        internal void PushRequest(ContextItem item)
        {
            queue.Enqueue(item);
            requestAdded.Set();
        }

        public RouteEnumerator(string route)
        {
            Route = route;
            Server.RouteObjects.TryAdd(route, this);
        }

        private bool disposed = false;

        public async ValueTask DisposeAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                Server.RouteObjects.Remove(Route, out _);
                disposed = true;
                foreach (var item in queue)
                    item.Handled.Set();
            });
        }

        public async ValueTask<bool> MoveNextAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                if (disposed)
                    return false;

                if (_current != null)
                {
                    _current.Handled.Set();
                }

            Get:

                if (queue.TryDequeue(out ContextItem item))
                {
                    _current = item;
                    return true;
                }
                else
                {
                    requestAdded.WaitOne();
                    goto Get;
                }
            });
        }
    }
}
