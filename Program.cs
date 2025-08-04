using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using NWebApp.Context;
using NWebApp.Middleware;
using NWebApp.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    options.EnableSensitiveDataLogging(true);
});

builder.Services.AddControllers();

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("FixedWindows", fixOpts =>
    {
        fixOpts.PermitLimit = 1;
        fixOpts.QueueLimit = 0;
        fixOpts.Window = TimeSpan.FromSeconds(15);
    });
});

var app = builder.Build();


app.UseRateLimiter();
app.MapControllers();
app.UseMiddleware<TestMiddleware>();
app.MapGet("/", () => "Hello World!");
var context = app.Services.CreateScope()
    .ServiceProvider.GetService<DataContext>();
await SeedData.SeedDatabase(context!);
app.Run();