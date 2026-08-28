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
        if (context.Request.Headers.TryGetValue("X-User-Id", out var value) &&
            Guid.TryParse(value, out var userId))
        {
            currentUserContext.SetUserId(userId);
        }

        await _next(context);
    }
}