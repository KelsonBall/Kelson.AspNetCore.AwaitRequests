using Microsoft.AspNetCore.Http;

namespace Kelson.AspNetCore.AwaitRequests
{
    public class Get : Route
    {
        protected Get(string path) : base(path) { }

        internal override bool MatchesRequest(HttpRequest request)
            => request.Method == "GET" && request.Path.Value.EndsWith(Path);

        public static explicit operator Get(string path) => new Get(path);
    }
}
