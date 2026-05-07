using Microsoft.AspNetCore.Mvc.Filters;

namespace Activity.API.Filters;

public class ExtractUserIdFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var userId = context.HttpContext.User.FindFirst("sub")?.Value
                  ?? context.HttpContext.User.FindFirst("userId")?.Value;

        if (!string.IsNullOrWhiteSpace(userId))
            context.HttpContext.Items["UserId"] = userId;
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}
