using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;

namespace BACKEND.Helpers
{
    public class BlackListMiddleware
    {
        private readonly RequestDelegate _next;

        public BlackListMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync( HttpContext context, BlackListService blackListService)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var jti = context.User.FindFirst( JwtRegisteredClaimNames.Jti)?.Value;

                if (jti != null && await blackListService.IsBlackListedAsync(jti))
                {
                    var problem = new ProblemDetails
                    {
                        Type = "https://httpstatuses.io/401",
                        Title = "Token revocado.",
                        Status = 401,
                        Detail = "El token de acceso ha sido invalidado. Inicie sesión nuevamente."
                    };
                    context.Response.StatusCode = 401;
                    context.Response.ContentType = "application/problem+json";
                    await context.Response.WriteAsJsonAsync(problem);
                    
                    return;
                }
            }

            await _next(context);
        }
    }
}
