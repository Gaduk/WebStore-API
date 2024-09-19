using Microsoft.AspNetCore.Diagnostics;

namespace Web_API;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError("{ExceptionMessage}", exception.Message);
        await httpContext.Response.WriteAsJsonAsync(exception.Message, cancellationToken);
        return true;
    }
}