using IdentityServer.Data;
using IdentityServer.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? issuerUri = builder.Configuration["IdentityServer:IssuerUri"];
string migrationsAssembly = typeof(Program).Assembly.GetName().Name!;

string rawConnectionString = builder.Configuration.GetConnectionString("IdentityDb")
    ?? throw new InvalidOperationException("IdentityDb connection string is not configured.");

string connectionString = new NpgsqlConnectionStringBuilder(rawConnectionString).ConnectionString;

builder.Services.AddRazorPages();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularClient", policy =>
    {
        string[] allowedOrigins = builder.Configuration
            .GetSection("Cors:AllowedOrigins")
            .Get<string[]>() ?? [];

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo("/app/keys"))
    .SetApplicationName("IdentityServer");

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Реєструємо Config через DI щоб SeedUsers міг його використовувати
builder.Services.AddSingleton<Config>();

IIdentityServerBuilder identityServerBuilder = builder.Services
    .AddIdentityServer(options =>
    {
        if (!string.IsNullOrEmpty(issuerUri))
            options.IssuerUri = issuerUri;

        options.UserInteraction.LoginUrl = "/Account/Login";
        options.UserInteraction.LogoutUrl = "/Account/Logout";
    })
    .AddConfigurationStore(opt =>
    {
        opt.ConfigureDbContext = b =>
            b.UseNpgsql(connectionString, sql =>
                sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(opt =>
    {
        opt.ConfigureDbContext = b =>
            b.UseNpgsql(connectionString, sql =>
                sql.MigrationsAssembly(migrationsAssembly));
        // Автоматично чистить прострочені токени
        opt.EnableTokenCleanup = true;
        opt.TokenCleanupInterval = 3600;
    })
    .AddAspNetIdentity<ApplicationUser>()
    .AddProfileService<CustomProfileService>(); // для roles в токені

// ⬇ ПІДПИС — різна логіка для dev та prod
string? certPassword = builder.Configuration["IdentityServer:SigningCertPassword"];
string? certPath = builder.Configuration["IdentityServer:SigningCertPath"];

if (!string.IsNullOrEmpty(certPassword) && !string.IsNullOrEmpty(certPath))
{
    var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(certPath, certPassword);
    identityServerBuilder.AddSigningCredential(cert);
}
else
{
    identityServerBuilder.AddDeveloperSigningCredential(persistKey: true);
}

if (!builder.Environment.IsDevelopment())
{
    builder.Services.Configure<CookieAuthenticationOptions>(
        IdentityServerConstants.DefaultCookieAuthenticationScheme,
        options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });
}

WebApplication app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
    KnownNetworks = { },
    KnownProxies = { }
});

if (!app.Environment.IsDevelopment())
{
    app.Use((ctx, next) =>
    {
        ctx.Request.Scheme = "https";
        return next();
    });
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAngularClient");
app.UseIdentityServer();
app.UseAuthorization();
app.MapRazorPages();

await app.InitializeDatabaseAsync();

app.Run();