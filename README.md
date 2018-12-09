# Kelson.AspNetCore.AwaitRequests

```csharp
// Create a web server and add some route handlers

await new Server(
  "https://localhost:5000",
  async () => 
  {
    await foreach (var request in (Get)"/hello")
      await request.Response.WriteAsync("Hello!");
  })
  .RunAsync();
```
