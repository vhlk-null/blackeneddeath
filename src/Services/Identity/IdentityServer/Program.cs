using IdentityServer.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Config config = new Config(builder.Configuration);
string? issuerUri = builder.Configuration["IdentityServer:IssuerUri"];
string? migrationsAssembly = typeof(Program).Assembly.GetName().Name;

string rawConnectionString = builder.Configuration.GetConnectionString("IdentityDb")
    ?? Environment.GetEnvironmentVariable("DATABASE_URL")
    ?? throw new InvalidOperationException("IdentityDb connection string is not configured.");

Console.WriteLine($"[DEBUG] Raw ConnectionString: {rawConnectionString}");

string connectionString = ConvertConnectionString(rawConnectionString);

static string ConvertConnectionString(string value)
{
    if (!value.StartsWith("postgresql://") && !value.StartsWith("postgres://"))
        return value;

    var uri = new Uri(value);
    var userInfo = uri.UserInfo.Split(':', 2);
    return $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={userInfo[0]};Password={userInfo[1]};Trust Server Certificate=true";
}

builder.Services.AddRazorPages();

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
    identityServerBuilder.AddDeveloperSigningCredential(persistKey: false);
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

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.MapRazorPages();

await app.InitializeDatabaseAsync();

app.Run();
