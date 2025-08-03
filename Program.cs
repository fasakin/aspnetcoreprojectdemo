using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
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

var app = builder.Build();



app.MapControllers();
app.UseMiddleware<TestMiddleware>();
app.MapGet("/", () => "Hello World!");
var context = app.Services.CreateScope()
    .ServiceProvider.GetService<DataContext>();
await SeedData.SeedDatabase(context!);
app.Run();