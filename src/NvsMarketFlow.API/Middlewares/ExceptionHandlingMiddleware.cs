using FluentValidation;
using NvsMarketFlow.Application.Exceptions;

namespace NvsMarketFlow.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(
        RequestDelegate next,
        ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            LogException(ex);

            await HandleExceptionAsync(context, ex);
        }
    }

    private void LogException(Exception ex)
    {
        var isExpectedException = ex is ValidationException
            or ArgumentException
            or InvalidOperationException
            or NotFoundException
            or UnauthorizedException
            or BadRequestException
            or DuplicateFieldException
            or ForbiddenException
            or CategoryHasLinkedProductsException;

        if (isExpectedException)
        {
            _logger.LogWarning(ex, "Handled exception: {Message}", ex.Message);
        }
        else
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
        }
    }

    private static async Task HandleExceptionAsync(
        HttpContext context,
        Exception ex)
    {
        context.Response.ContentType = "application/json";

        var statusCode = ex switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            UnauthorizedException => StatusCodes.Status401Unauthorized,
            BadRequestException => StatusCodes.Status400BadRequest,
            DuplicateFieldException => StatusCodes.Status409Conflict,
            CategoryHasLinkedProductsException => StatusCodes.Status409Conflict,
            ForbiddenException => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.StatusCode = statusCode;

        if (ex is ValidationException validationException)
        {
            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key.Replace("Request.", ""),
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            await context.Response.WriteAsJsonAsync(new
            {
                status = statusCode,
                errors
            });

            return;
        }

        var response = new
        {
            status = statusCode,
            error = statusCode == StatusCodes.Status500InternalServerError
                ? "Internal server error."
                : ex.Message
        };

        await context.Response.WriteAsJsonAsync(response);
    }
}