using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Kelson.AspNetCore.AwaitRequests
{
    public abstract class ContextRoute : IAsyncEnumerable<HttpContext>
    {
        public readonly string Route;        

        protected ContextRoute(string route)
        {
            Route = route;
            Enumerator = new RouteEnumerator(route);
            Server.RouteObjects.TryAdd(route, this);
        }

        internal readonly RouteEnumerator Enumerator;

        public IAsyncEnumerator<HttpContext> GetAsyncEnumerator() => Enumerator;

        internal abstract bool MatchesRequest(HttpRequest request);
    }
}
