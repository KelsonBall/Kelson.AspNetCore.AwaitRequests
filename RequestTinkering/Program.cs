namespace RequestTinkering
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Kelson.AspNetCore.AwaitRequests;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new Server("https://localhost:5000",
                async () =>
                {
                    await foreach (var context in (Get)"/hello")
                        await context.Response.WriteAsync("Hello world");
                },
                async () =>
                {
                    await foreach (var context in (Get)"/goodbye")
                        await context.Response.WriteAsync("So long");
                })
                .RunAsync();            
        }        
    }
}
