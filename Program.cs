using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using NWebApp.Context;
using NWebApp.Middleware;
using NWebApp.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ProductConnection"]);
    options.EnableSensitiveDataLogging(true);
});

builder.Services.AddControllers()
    .AddNewtonsoftJson()
    .AddXmlDataContractSerializerFormatters();

builder.Services.Configure<MvcOptions>(options =>
{
    options.RespectBrowserAcceptHeader = true;
    options.ReturnHttpNotAcceptable = true;
});

builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "NWebApp v1", Version = "v1", Description = "document about my NWebApp"
    });
    
    
    opt.SwaggerDoc("v2", new OpenApiInfo()
    {
        Title = "NWebApp v2", Version = "v2"
    });
    opt.DocInclusionPredicate((docName, api) => api.GroupName == docName);
    
    
});


builder.Services.AddOutputCache(options =>
{
    options.AddPolicy("30Secs", b =>
    {
        b.Cache();
        b.Expire(TimeSpan.FromSeconds(30));
    });
});
// builder.Services.Configure<JsonOptions>(options =>
// {
//     options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
// });

builder.Services.Configure<MvcNewtonsoftJsonOptions>(options =>
{
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
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


app.UseSwagger(opt =>
{
    opt.RouteTemplate = "/novamarks/{documentName}/epson.json";
    Console.WriteLine($"Route Template: {opt.RouteTemplate}");
    
});
app.UseSwaggerUI(opt =>
{
    opt.SwaggerEndpoint("/novamarks/v1/epson.json", "NWebApp v1");
    opt.SwaggerEndpoint("/novamarks/v2/epson.json", "NWebApp v2");
    opt.RoutePrefix = "novamarks";

   
});
app.UseRateLimiter();
app.UseOutputCache();
app.MapControllers();
app.UseMiddleware<TestMiddleware>();
app.MapGet("/", () => "Hello World!");
var context = app.Services.CreateScope()
    .ServiceProvider.GetService<DataContext>();
await SeedData.SeedDatabase(context!);
app.Run();