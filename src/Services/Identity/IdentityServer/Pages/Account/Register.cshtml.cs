using System.Security.Claims;
using Duende.IdentityServer.Services;
using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Pages.Account;

public class RegisterModel(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IIdentityServerInteractionService interaction) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string? ReturnUrl { get; set; }

    [BindProperty]
    public string Username { get; set; } = string.Empty;

    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Passwords do not match.";
            return Page();
        }

        ApplicationUser user = new()
        {
            UserName = Username,
            Email = Email
        };

        IdentityResult result = await userManager.CreateAsync(user, Password);

        if (!result.Succeeded)
        {
            ErrorMessage = string.Join(" ", result.Errors.Select(e => e.Description));
            return Page();
        }

        await userManager.AddClaimAsync(user, new Claim("role", "user"));

        await signInManager.SignInAsync(user, isPersistent: false);

        AuthorizationRequest? context = await interaction.GetAuthorizationContextAsync(ReturnUrl);

        if (context != null || Url.IsLocalUrl(ReturnUrl))
            return Redirect(ReturnUrl!);

        return Redirect("/");
    }
}
