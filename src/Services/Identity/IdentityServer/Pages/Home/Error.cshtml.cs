using Duende.IdentityServer.Services;

namespace IdentityServer.Pages.Home;

public class ErrorModel(IIdentityServerInteractionService interaction) : PageModel
{
    public string? ErrorDescription { get; set; }

    public async Task OnGetAsync(string? errorId)
    {
        if (errorId is not null)
        {
            ErrorMessage error = await interaction.GetErrorContextAsync(errorId);
            ErrorDescription = error?.ErrorDescription ?? error?.Error;
        }
    }
}
