using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Yarp.ReverseProxy.Transforms;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        string authority = builder.Configuration["IdentityServer:Authority"]!;
        options.RequireHttpsMetadata = false;
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true
        };

        string metadataAddress = $"{authority}/.well-known/openid-configuration";
        HttpClientHandler httpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        };
        HttpDocumentRetriever retriever = new HttpDocumentRetriever(new HttpClient(httpHandler)) { RequireHttps = false };

        options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
            metadataAddress,
            new OpenIdConnectConfigurationRetriever(),
            retriever);
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
            HttpContext httpContext = transformContext.HttpContext;
            AuthenticateResult authResult = await httpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            if (!authResult.Succeeded)
                return;

            ClaimsPrincipal user = authResult.Principal;

            string? userId = user.FindFirst("sub")?.Value;
            if (userId is not null)
                transformContext.ProxyRequest.Headers.TryAddWithoutValidation("X-User-Id", userId);

            List<string> roles = user.FindAll("role").Select(c => c.Value).ToList();
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
    string[] origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? [];
    options.AddDefaultPolicy(policy =>
        policy.SetIsOriginAllowed(origin =>
              {
                  Uri uri = new Uri(origin);
                  return uri.Host == "localhost" || origins.Contains(origin);
              })
              .AllowAnyHeader()
              .AllowAnyMethod());
});

WebApplication app = builder.Build();

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
