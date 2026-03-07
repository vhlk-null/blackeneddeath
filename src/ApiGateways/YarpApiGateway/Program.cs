var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// Add services to the container. 
var app = builder.Build();

// Configure HTTP request pipeline.

app.MapReverseProxy();

app.MapGet("/", () => "Hello World!");

app.Run();
