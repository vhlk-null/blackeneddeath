using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Pages.Account;

public class LoginModel(
    IIdentityServerInteractionService interaction,
    SignInManager<ApplicationUser> signInManager) : PageModel
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
        AuthorizationRequest? context = await interaction.GetAuthorizationContextAsync(ReturnUrl);

        if (!ModelState.IsValid)
            return Page();

        Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(
            Username, Password, RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            ErrorMessage = "Invalid username or password.";
            return Page();
        }

        if (context != null || Url.IsLocalUrl(ReturnUrl))
            return Redirect(ReturnUrl ?? "/");

        return Redirect("/");
    }
}
