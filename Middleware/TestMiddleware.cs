using NWebApp.Context;

namespace NWebApp.Middleware;

public class TestMiddleware
{
    private RequestDelegate _requestDelegate;

    public TestMiddleware(RequestDelegate requestDelegate)
    {
        _requestDelegate = requestDelegate;
    }

    public async Task Invoke(HttpContext context, DataContext dataContext)
    {
        if (context.Request.Path == "/test")
        {
            await context.Response.WriteAsync($"There are {dataContext.Products.Count()} products\n");
            await context.Response.WriteAsync($"There are {dataContext.Categories.Count()} categories\n");
            await context.Response.WriteAsync($"There are {dataContext.Suppliers.Count()} suppliers\n");
        }

        if (_requestDelegate != null)
        {
            await _requestDelegate(context);
        }
    }
}