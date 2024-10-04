using Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using static System.String;

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
            case FluentValidation.ValidationException validationException:
            {
                problemDetails.Title = "Validation failed";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                var validationErrors = 
                    validationException.Errors
                        .Select(error => error.ErrorMessage).ToList();
                problemDetails.Extensions.Add("errors", validationErrors);
                break;
            }
            case BadRequestException badRequestException:
            {
                problemDetails.Title = "Bad request";
                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                problemDetails.Extensions.Add("error", badRequestException.Message);
                break;
            }
            case UnauthorizedException unauthorizedException:
            {
                problemDetails.Title = "Unauthorized";
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                problemDetails.Extensions.Add("error", unauthorizedException.Message);
                break;
            }
            case ForbiddenException:
            {
                problemDetails.Title = "Access denied";
                httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                break;
            }
            case NotFoundException notFoundException:
            {
                problemDetails.Title = "Not found";
                httpContext.Response.StatusCode = StatusCodes.Status404NotFound;
                problemDetails.Extensions.Add("error", notFoundException.Message);
                break;
            }
            case ConflictException conflictException:
            {
                problemDetails.Title = "Conflict";
                httpContext.Response.StatusCode = StatusCodes.Status409Conflict;
                problemDetails.Extensions.Add("error", conflictException.Message);
                break;
            }
            default:
                problemDetails.Title = exception.GetType().Name;
                problemDetails.Extensions.Add("error", exception.Message);
                break;
        }
        problemDetails.Status = httpContext.Response.StatusCode;

        problemDetails.Extensions.TryGetValue("errors", out var errorsObj );
        problemDetails.Extensions.TryGetValue("error", out var errorObj);
        var errors = Join(". ", errorsObj as List<string> ?? Enumerable.Empty<string>());
        errors = Join("", errors, errorObj);
        
        logger.LogWarning("{title}. {errors}", problemDetails.Title, errors);
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}