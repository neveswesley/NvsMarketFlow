using System.Security.Claims;
using NvsMarketFlow.Application.Common;

namespace NvsMarketFlow.API.Middlewares;

public class CurrentUserMiddleware
{
    private readonly RequestDelegate _next;

    public CurrentUserMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentUserContext currentUserContext)
    {
        var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var roleClaim = context.User.FindFirst(ClaimTypes.Role)?.Value;

        if (userIdClaim is not null && Guid.TryParse(userIdClaim, out var userIdFromToken))
        {
            currentUserContext.SetUserId(userIdFromToken);

            if (roleClaim is not null)
                currentUserContext.SetRole(roleClaim);
        }
        else if (context.Request.Headers.TryGetValue("X-User-Id", out var headerValue) &&
                 Guid.TryParse(headerValue, out var userIdFromHeader))
        {
            currentUserContext.SetUserId(userIdFromHeader);
        }

        await _next(context);
    }
}