using IdentityServer.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.HttpOverrides;
using Npgsql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string? issuerUri = builder.Configuration["IdentityServer:IssuerUri"];
string? migrationsAssembly = typeof(Program).Assembly.GetName().Name;

string rawConnectionString = builder.Configuration.GetConnectionString("IdentityDb")
    ?? throw new InvalidOperationException("IdentityDb connection string is not configured.");

var csBuilder = new NpgsqlConnectionStringBuilder(rawConnectionString);

string connectionString = csBuilder.ConnectionString;
Console.WriteLine($"[DEBUG] Final ConnectionString: {connectionString}");

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


IIdentityServerBuilder identityServerBuilder = builder.Services.AddIdentityServer(options =>
    {
        if (!string.IsNullOrEmpty(issuerUri))
            options.IssuerUri = issuerUri;

        options.UserInteraction.LoginUrl = "/Account/Login";
        options.UserInteraction.LogoutUrl = "/Account/Logout";
    })
    .AddConfigurationStore(opt =>
    {
        opt.ConfigureDbContext = b => b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddOperationalStore(opt =>
    {
        opt.ConfigureDbContext = b => b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddAspNetIdentity<ApplicationUser>();

if (builder.Environment.IsDevelopment())
    identityServerBuilder.AddDeveloperSigningCredential(persistKey: true);
else
    identityServerBuilder.AddDeveloperSigningCredential(persistKey: true, filename: "/app/keys/signing-credential.jwk");

if (!builder.Environment.IsDevelopment())
{
    builder.Services.Configure<CookieAuthenticationOptions>(IdentityServerConstants.DefaultCookieAuthenticationScheme, options =>
    {
        options.Cookie.SameSite = SameSiteMode.None;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });
}

WebApplication app = builder.Build();

ForwardedHeadersOptions forwardedOptions = new()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
forwardedOptions.KnownNetworks.Clear();
forwardedOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardedOptions);

app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAngularClient");
app.UseIdentityServer();
app.UseAuthorization();
app.MapRazorPages();

await app.InitializeDatabaseAsync();

app.Run();
