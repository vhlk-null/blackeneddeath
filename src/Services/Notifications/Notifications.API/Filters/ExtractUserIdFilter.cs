using Microsoft.AspNetCore.Mvc.Filters;

namespace Notifications.API.Filters;

public class ExtractUserIdFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.Request.Query.TryGetValue("userId", out var queryUserId))
        {
            context.HttpContext.Items["UserId"] = queryUserId.ToString();
            return;
        }

        var userId = context.HttpContext.User.FindFirst("sub")?.Value
            ?? context.HttpContext.User.FindFirst("userId")?.Value
            ?? context.HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (userId is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        context.HttpContext.Items["UserId"] = userId;
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
