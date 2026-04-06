using IdentityServer;

var builder = WebApplication.CreateBuilder(args);

var issuerUri = builder.Configuration["IdentityServer:IssuerUri"];

builder.Services.AddIdentityServer(options =>
    {
        if (!string.IsNullOrEmpty(issuerUri))
            options.IssuerUri = issuerUri;
    })
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddDeveloperSigningCredential(persistKey: false);


var app = builder.Build();

app.UseRouting();
app.UseIdentityServer();

app.Run();
