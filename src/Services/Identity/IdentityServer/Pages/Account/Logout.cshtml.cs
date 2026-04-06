using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Pages.Account;

public class LogoutModel(IIdentityServerInteractionService interaction) : PageModel
{
    public string? PostLogoutRedirectUri { get; set; }
    public bool ShowSignedOutPage { get; set; }

    public async Task<IActionResult> OnGetAsync(string? logoutId)
    {
        var context = await interaction.GetLogoutContextAsync(logoutId);

        await HttpContext.SignOutAsync(
            Duende.IdentityServer.IdentityServerConstants.DefaultCookieAuthenticationScheme);

        ShowSignedOutPage = true;
        PostLogoutRedirectUri = context?.PostLogoutRedirectUri;

        return Page();
    }
}
