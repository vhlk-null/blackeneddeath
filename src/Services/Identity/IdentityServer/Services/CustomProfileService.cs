using Duende.IdentityServer.AspNetIdentity;
using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services;

public class CustomProfileService(
    UserManager<ApplicationUser> userManager,
    IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory)
    : ProfileService<ApplicationUser>(userManager, claimsFactory)
{
    protected override async Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
    {
        var principal = await GetUserClaimsAsync(user);
        var claims = principal.Claims.ToList();

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role =>
            new Claim(JwtClaimTypes.Role, role)));

        context.AddRequestedClaims(claims);
    }
}