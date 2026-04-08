using IdentityServer.Data;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Config config = new Config(builder.Configuration);
string? issuerUri = builder.Configuration["IdentityServer:IssuerUri"];
string? connectionString = builder.Configuration.GetConnectionString("IdentityDb");
string? migrationsAssembly = typeof(Program).Assembly.GetName().Name;

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

if (app.Environment.IsDevelopment())
    await app.InitializeDatabaseAsync();

app.Run();
