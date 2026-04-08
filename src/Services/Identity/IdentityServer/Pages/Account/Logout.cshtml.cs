using Duende.IdentityServer.Services;
using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Account;

public class LogoutModel(
    IIdentityServerInteractionService interaction,
    SignInManager<ApplicationUser> signInManager) : PageModel
{
    public string? PostLogoutRedirectUri { get; set; }
    public bool ShowSignedOutPage { get; set; }

    public async Task<IActionResult> OnGetAsync(string? logoutId)
    {
        LogoutRequest? context = await interaction.GetLogoutContextAsync(logoutId);

        await signInManager.SignOutAsync();

        ShowSignedOutPage = true;
        PostLogoutRedirectUri = context?.PostLogoutRedirectUri;

        return Page();
    }
}
