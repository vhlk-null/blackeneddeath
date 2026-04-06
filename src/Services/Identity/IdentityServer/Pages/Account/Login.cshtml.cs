using Duende.IdentityServer.Services;
using Duende.IdentityServer.Test;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace IdentityServer.Pages.Account;

public class LoginModel(
    IIdentityServerInteractionService interaction,
    TestUserStore users) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public bool RememberMe { get; set; }

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var context = await interaction.GetAuthorizationContextAsync(ReturnUrl);

        if (!ModelState.IsValid)
            return Page();

        if (!users.ValidateCredentials(Username, Password))
        {
            ErrorMessage = "Invalid username or password.";
            return Page();
        }

        var user = users.FindByUsername(Username)!;

        var claims = new List<Claim>
        {
            new(System.Security.Claims.ClaimTypes.Name, user.Username)
        };
        claims.AddRange(user.Claims.Select(c => new Claim(c.Type, c.Value)));

        var principal = new ClaimsPrincipal(new ClaimsIdentity(
            claims,
            Duende.IdentityServer.IdentityServerConstants.DefaultCookieAuthenticationScheme));

        var props = new AuthenticationProperties();
        if (RememberMe)
        {
            props.IsPersistent = true;
            props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30);
        }

        await HttpContext.SignInAsync(
            Duende.IdentityServer.IdentityServerConstants.DefaultCookieAuthenticationScheme,
            principal,
            props);

        if (context != null || Url.IsLocalUrl(ReturnUrl))
            return Redirect(ReturnUrl ?? "/");

        return Redirect("/");
    }
}
