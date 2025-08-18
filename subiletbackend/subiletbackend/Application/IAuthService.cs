using SubiletBackend.Domain;

namespace SubiletBackend.Application
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hash, string password);
        string GenerateJwtToken(User user);
        string GenerateRefreshToken();
    }
} 