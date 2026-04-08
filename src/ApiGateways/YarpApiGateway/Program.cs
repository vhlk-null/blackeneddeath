using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var authority = builder.Configuration["IdentityServer:Authority"]!;
        var jwksUri = builder.Configuration["IdentityServer:JwksUri"];
        options.RequireHttpsMetadata = false;
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true
        };

        var metadataAddress = $"{authority}/.well-known/openid-configuration";
        var httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        var retriever = new HttpDocumentRetriever(new HttpClient(httpHandler)) { RequireHttps = false };

        if (jwksUri is not null)
        {
            // Завантажуємо metadata але підміняємо jwks_uri на внутрішній Docker URI
            options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                metadataAddress,
                new OpenIdConnectConfigurationRetriever(),
                retriever);

            options.TokenValidationParameters.IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
            {
                var jwks = new HttpClient(httpHandler).GetStringAsync(jwksUri).GetAwaiter().GetResult();
                var keySet = new Microsoft.IdentityModel.Tokens.JsonWebKeySet(jwks);
                return keySet.GetSigningKeys();
            };
        }
        else
        {
            options.Authority = authority;
            options.MetadataAddress = metadataAddress;
            options.BackchannelHttpHandler = httpHandler;
        }
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("authenticated", policy => policy.RequireAuthenticatedUser());
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(context =>
    {
        context.AddRequestTransform(async transformContext =>
        {
            var httpContext = transformContext.HttpContext;
            var authResult = await httpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
                return;

            var user = authResult.Principal;

            var userId = user.FindFirst("sub")?.Value;
            if (userId is not null)
                transformContext.ProxyRequest.Headers.TryAddWithoutValidation("X-User-Id", userId);

            var roles = user.FindAll("role").Select(c => c.Value).ToList();
            if (roles.Count > 0)
                transformContext.ProxyRequest.Headers.TryAddWithoutValidation("X-User-Role", string.Join(",", roles));
        });
    });

builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
    {
        options.Window = builder.Configuration.GetValue("RateLimiter:Window", TimeSpan.FromSeconds(10));
        options.PermitLimit = builder.Configuration.GetValue("RateLimiter:PermitLimit", 5);
    });
});

builder.Services.AddCors(options =>
{
    var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
    options.AddDefaultPolicy(policy =>
        policy.SetIsOriginAllowed(origin =>
              {
                  var uri = new Uri(origin);
                  return uri.Host == "localhost" || origins.Contains(origin);
              })
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();
app.MapReverseProxy();

app.MapGet("/", () => "Hello World!");

app.Run();

namespace YarpApiGateway
{
    public partial class Program { }
}
