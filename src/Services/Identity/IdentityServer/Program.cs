using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

Config config = new Config(builder.Configuration);
string? issuerUri = builder.Configuration["IdentityServer:IssuerUri"];

builder.Services.AddRazorPages();

string? migrationsAssembly = typeof(Program).Assembly.GetName().Name;
string? connectionString = builder.Configuration.GetConnectionString("IdentityDb");

builder.Services.AddIdentityServer(options =>
    {
        if (!string.IsNullOrEmpty(issuerUri))
            options.IssuerUri = issuerUri;

        options.UserInteraction.LoginUrl = "/Account/Login";
        options.UserInteraction.LogoutUrl = "/Account/Logout";
    })
    //.AddInMemoryClients(config.Clients)
    //.AddInMemoryApiScopes(config.ApiScopes)
    //.AddInMemoryApiResources(config.ApiResources)
    //.AddInMemoryIdentityResources(config.IdentityResources)
    .AddConfigurationStore(opt =>
    {
        opt.ConfigureDbContext = b => b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
    }).AddOperationalStore(opt =>
    {
        opt.ConfigureDbContext = b => b.UseNpgsql(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
    })
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

WebApplication app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseIdentityServer();
app.UseAuthorization();
app.MapRazorPages();

if (app.Environment.IsDevelopment())
    await app.InitializeDatabaseAsync();

app.Run();
