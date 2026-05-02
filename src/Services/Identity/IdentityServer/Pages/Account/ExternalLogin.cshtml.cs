using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityServer.Pages.Account;

public class ExternalLoginModel(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IIdentityServerInteractionService interaction) : PageModel
{
    public IActionResult OnGet() => RedirectToPage("/Account/Login");

    public IActionResult OnPost(string provider, string? returnUrl = null)
    {
        string redirectUrl = Url.Page("/Account/ExternalLogin", "Callback", new { returnUrl })!;
        var properties = signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string? returnUrl = null, string? remoteError = null)
    {
        if (remoteError is not null)
            return RedirectToPage("/Account/Login", new { error = $"External provider error: {remoteError}" });

        ExternalLoginInfo? info = await signInManager.GetExternalLoginInfoAsync();
        if (info is null)
            return RedirectToPage("/Account/Login");

        var result = await signInManager.ExternalLoginSignInAsync(
            info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

        if (result.Succeeded)
            return Redirect(returnUrl ?? "/");

        // No local account — create one
        string? email = info.Principal.FindFirstValue(ClaimTypes.Email);
        string? name = info.Principal.FindFirstValue(ClaimTypes.Name);

        if (email is null)
            return RedirectToPage("/Account/Login", new { error = "Google account has no email." });

        string username = email.Split('@')[0];

        var user = new ApplicationUser
        {
            UserName = username,
            Email = email,
            EmailConfirmed = true
        };

        var createResult = await userManager.CreateAsync(user);
        if (!createResult.Succeeded)
            return RedirectToPage("/Account/Login", new { error = "Failed to create account." });

        await userManager.AddToRoleAsync(user, "user");

        if (name is not null)
            await userManager.AddClaimAsync(user, new Claim(JwtClaimTypes.Name, name));

        await userManager.AddLoginAsync(user, info);
        await signInManager.SignInAsync(user, isPersistent: false);

        return Redirect(returnUrl ?? "/");
    }
}
