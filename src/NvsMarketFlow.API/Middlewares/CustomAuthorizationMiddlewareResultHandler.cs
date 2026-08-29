using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;

namespace NvsMarketFlow.API.Middlewares;

public class CustomAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Challenged)
        {
            var authFeature = context.Features.Get<IAuthenticateResultFeature>();
            var failureReason = authFeature?.AuthenticateResult?.Failure?.Message;

            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                status = 401,
                error = failureReason?.Contains("expired") == true
                    ? "Seu token expirou. Faça login novamente ou use o refresh token."
                    : "Token ausente ou inválido."
            });

            return;
        }

        if (authorizeResult.Forbidden)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                status = 403,
                error = "Você não tem permissão para executar essa ação."
            });

            return;
        }
        
        await _defaultHandler.HandleAsync(next, context, policy, authorizeResult);
    }
}