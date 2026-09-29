using Tourism.Core.Exceptions;

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
        try { await _next(context); }
        catch (NotFoundException ex) { await HandleAsync(context, 404, ex.Message); }
        catch (ForbiddenException ex) { await HandleAsync(context, 403, ex.Message); }
        catch (ValidationException ex) { await HandleAsync(context, 400, ex.Message); }
        catch (ConflictException ex) { await HandleAsync(context, 409, ex.Message); }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message); 
            await HandleAsync(context, 500, "Internal server error");
        }
    }

    private static async Task HandleAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { statusCode, error = message });
    }
}