using Microsoft.EntityFrameworkCore;
using NWebApp.Context;
using NWebApp.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    options.EnableSensitiveDataLogging(true);
});

var app = builder.Build();

app.UseMiddleware<TestMiddleware>();
app.MapGet("/", () => "Hello World!");
var context = app.Services.CreateScope()
    .ServiceProvider.GetService<DataContext>();
await SeedData.SeedDatabase(context!);
app.Run();