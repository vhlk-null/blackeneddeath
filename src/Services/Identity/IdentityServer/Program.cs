using IdentityServer;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

var config = new Config(builder.Configuration);
var issuerUri = builder.Configuration["IdentityServer:IssuerUri"];

builder.Services.AddRazorPages();

builder.Services.AddIdentityServer(options =>
    {
        if (!string.IsNullOrEmpty(issuerUri))
            options.IssuerUri = issuerUri;

        options.UserInteraction.LoginUrl = "/Account/Login";
        options.UserInteraction.LogoutUrl = "/Account/Logout";
    })
    .AddInMemoryClients(config.Clients)
    .AddInMemoryApiScopes(config.ApiScopes)
    .AddInMemoryApiResources(config.ApiResources)
    .AddInMemoryIdentityResources(config.IdentityResources)
    .AddTestUsers(config.TestUsers)
    .AddDeveloperSigningCredential(persistKey: false);

if (!builder.Environment.IsDevelopment())
{
    builder.Services.Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme, options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
}

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.MapRazorPages();

app.Run();
