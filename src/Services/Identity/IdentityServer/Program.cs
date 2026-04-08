using IdentityServer.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Npgsql;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Config config = new Config(builder.Configuration);
string? issuerUri = builder.Configuration["IdentityServer:IssuerUri"];
string? connectionString = builder.Configuration.GetConnectionString("IdentityDb");
string? migrationsAssembly = typeof(Program).Assembly.GetName().Name;

// If the connection string is a PostgreSQL URI (e.g. DATABASE_URL from Railway),
// parse and convert it to Npgsql key=value format.
if (!string.IsNullOrEmpty(connectionString) && connectionString.StartsWith("postgresql://"))
{
    var uri = new Uri(connectionString);
    var npgsqlBuilder = new NpgsqlConnectionStringBuilder
    {
        Host = uri.Host,
        Port = uri.Port == -1 ? 5432 : uri.Port,
        Database = uri.AbsolutePath.TrimStart('/'),
        Username = uri.UserInfo?.Split(':')[0],
        Password = uri.UserInfo?.Split(':').Length > 1 ? uri.UserInfo.Split(':')[1] : null
    };
    connectionString = npgsqlBuilder.ConnectionString;
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
