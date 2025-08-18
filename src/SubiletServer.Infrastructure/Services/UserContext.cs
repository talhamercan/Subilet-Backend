using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SubiletServer.Application.Services;

namespace SubiletServer.Infrastructure.Services
{
    internal sealed class UserContext(
        IHttpContextAccessor httpContextAccessor) : IUserContext
    {
     
        public Guid GetUserId()
        {
            var httpContext = httpContextAccessor.HttpContext;
            var claims = httpContext.User.Claims;
            string? userId = claims.FirstOrDefault(i => i.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                throw new ArgumentException("kullanıcı bilgisi bulunamadı");
            }
            try
            {
                Guid id = Guid.Parse(userId);
                return id;
            }
            catch
            {
                throw new ArgumentException("kullanıcı uygun guid formatında değil");
            }
        }
    }
}