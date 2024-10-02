using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Presentation;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Instance = httpContext.Request.Path
        };
        
        switch (exception)
        {
            case FluentValidation.ValidationException fluentException:
            {
                problemDetails.Title = "Validation failed";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                var validationErrors = 
                    fluentException.Errors
                        .Select(error => error.ErrorMessage).ToList();
                problemDetails.Extensions.Add("errors", validationErrors);
                break;
            }
            case UnauthorizedException unauthorizedException:
            {
                problemDetails.Title = unauthorizedException.Message;
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                break;
            }
            case ForbiddenException:
            {
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                break;
            }
            case NotFoundException notFoundException:
            {
                problemDetails.Title = notFoundException.Message;
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                break;
            }
            case ConflictException conflictException:
            {
                problemDetails.Title = conflictException.Message;
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                break;
            }
            default:
                problemDetails.Title = exception.Message;
                break;
        }
        problemDetails.Status = httpContext.Response.StatusCode;
        
        logger.LogWarning("{handledErrorMessage}", problemDetails.Title);
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}