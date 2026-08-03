namespace BACKEND.Exceptions;

/// <summary>
/// Clase base para todas las excepciones de dominio de la aplicación.
/// Transporta el código HTTP semántico para que el GlobalExceptionHandler
/// pueda mapearla sin necesidad de un switch por tipo.
/// </summary>
public abstract class AppException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}

/// <summary>HTTP 409 - Conflicto: el recurso ya existe o viola una regla de unicidad.</summary>
public sealed class ConflictException(string message)
    : AppException(message, StatusCodes.Status409Conflict);

/// <summary>HTTP 404 - El recurso solicitado no fue encontrado.</summary>
public sealed class NotFoundException(string message)
    : AppException(message, StatusCodes.Status404NotFound);

/// <summary>HTTP 401 - Credenciales inválidas o sesión expirada a nivel de negocio.</summary>
public sealed class UnauthorizedException(string message)
    : AppException(message, StatusCodes.Status401Unauthorized);

/// <summary>HTTP 400 - Validación de reglas de dominio que el ModelState no cubre.</summary>
public sealed class DomainValidationException(string message)
    : AppException(message, StatusCodes.Status400BadRequest);
