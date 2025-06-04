using System.Net;
using System.Text.Json;
using LibraryAPI.Domain.Exceptions;

namespace LibraryAPI.API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = new
        {
            StatusCode = GetStatusCode(exception),
            Message = GetMessage(exception),
            DetailedMessage = exception.Message
        };

        context.Response.StatusCode = (int)response.StatusCode;
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }

    private static HttpStatusCode GetStatusCode(Exception exception)
    {
        return exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            ValidationException => HttpStatusCode.BadRequest,
            UnauthorizedAccessException => HttpStatusCode.Unauthorized,
            _ => HttpStatusCode.InternalServerError
        };
    }

    private static string GetMessage(Exception exception)
    {
        return exception switch
        {
            NotFoundException => "Resource not found",
            ValidationException => "Validation error",
            UnauthorizedAccessException => "Unauthorized",
            _ => "Internal server error"
        };
    }
} 