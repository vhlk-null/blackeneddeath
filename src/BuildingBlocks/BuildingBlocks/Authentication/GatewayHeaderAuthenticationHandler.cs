using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.Extensions.Primitives;

namespace BuildingBlocks.Authentication;

public class GatewayHeaderAuthenticationOptions : AuthenticationSchemeOptions;

public class GatewayHeaderAuthenticationHandler(
    IOptionsMonitor<GatewayHeaderAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<GatewayHeaderAuthenticationOptions>(options, logger, encoder)
{
    protected override Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue("X-User-Id", out StringValues userId))
        {
            Logger.LogDebug("GatewayHeader: Missing X-User-Id header — treating as anonymous");
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        List<Claim> claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };

        if (Request.Headers.TryGetValue("X-User-Name", out StringValues userName))
            claims.Add(new Claim(ClaimTypes.Name, userName.ToString()));

        if (Request.Headers.TryGetValue("X-User-Email", out StringValues email))
            claims.Add(new Claim(ClaimTypes.Email, email.ToString()));

        if (Request.Headers.TryGetValue("X-User-Role", out StringValues roles))
        {
            foreach (string role in roles.ToString().Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                claims.Add(new Claim(ClaimTypes.Role, role));
        }

        ClaimsIdentity identity = new ClaimsIdentity(claims, Scheme.Name);
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
        AuthenticationTicket ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
