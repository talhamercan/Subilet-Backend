using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace SubiletServer.WebAPI.Middleware
{
    /// <summary>
    /// Angular frontend için JWT token middleware
    /// </summary>
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Angular'dan gelen Authorization header'ını kontrol et
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                if (!string.IsNullOrEmpty(token))
                {
                    // Token'ı context'e ekle
                    context.Items["JwtToken"] = token;
                }

                await _next(context);
            }
            catch (Exception ex)
            {
                // Hata durumunda Angular'a uygun response döndür
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    Success = false,
                    Message = "Bir hata oluştu",
                    Errors = new List<string> { ex.Message }
                };

                var jsonResponse = JsonSerializer.Serialize(errorResponse);
                await context.Response.WriteAsync(jsonResponse);
            }
        }
    }

    /// <summary>
    /// Middleware extension
    /// </summary>
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseJwtMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtMiddleware>();
        }
    }
} 