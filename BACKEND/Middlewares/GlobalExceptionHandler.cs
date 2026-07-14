using BACKEND.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND.Middlewares;
/// <summary>
/// Un único punto de entrada para todos los errores de la aplicación.
/// Devuelve respuestas estandarizadas en formato ProblemDetails (RFC 7807).
/// </summary>
internal sealed class GlobalExceptionHandler( ILogger<GlobalExceptionHandler> logger,IHostEnvironment env) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync( HttpContext httpContext, Exception exception,CancellationToken cancellationToken)
    {
        var (status, title) = exception switch
        {
            AppException ex => (ex.StatusCode, ex.Message),
            _ => (StatusCodes.Status500InternalServerError,
                  "Ocurrió un error interno. Por favor intenta más tarde.")
        };

        // Errores de negocio esperados → Warning. Errores inesperados → Error con stack trace.
        if (status >= 500)
            logger.LogError(exception,
                "Unhandled exception. Path: {Path} | TraceId: {TraceId}",
                httpContext.Request.Path,
                httpContext.TraceIdentifier);
        else
            logger.LogWarning(
                "Business exception [{Status}] {Message} | Path: {Path}",
                status, exception.Message, httpContext.Request.Path);

        var problem = new ProblemDetails
        {
            Status = status,
            Title  = title,
            Type   = $"https://httpstatuses.io/{status}",

            Detail = env.IsDevelopment() ? exception.StackTrace : null
        };
        problem.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);

        return true;
    }
}
