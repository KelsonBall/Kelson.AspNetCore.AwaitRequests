using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace Kelson.AspNetCore.AwaitRequests
{
    public abstract class Route : IAsyncEnumerable<HttpContext>
    {
        public readonly string Path;

        private readonly RouteEnumerator enumerator;

        protected Route(string path)
        {
            Path = path;
            enumerator = new RouteEnumerator(path);
        }

        public IAsyncEnumerator<HttpContext> GetAsyncEnumerator() => enumerator;

        internal abstract bool MatchesRequest(HttpRequest request);
    }
}
