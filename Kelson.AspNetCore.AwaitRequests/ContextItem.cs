using System.Threading;
using Microsoft.AspNetCore.Http;

namespace Kelson.AspNetCore.AwaitRequests
{
    internal class ContextItem
    {
        internal readonly HttpContext Context;

        internal readonly ManualResetEvent Handled;

        internal ContextItem(HttpContext context)
        {
            Context = context;
            Handled = new ManualResetEvent(false);
        }
    }
}
