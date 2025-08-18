using SubiletServer.Domain.Users;

namespace SubiletServer.Application.Services
{
    public interface IJwtProvider
    {
        string CreateToken(User user);
    string CreateToken(SiteUser user);
    }
}
