using SubiletServer.Application.Services;
using SubiletServer.Domain.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SubiletServer.Infrastructure.Services;

internal sealed class JwtProvider : IJwtProvider
{
    private readonly IConfiguration _configuration;

    public JwtProvider(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(User user)
    {
        return CreateTokenInternal(user.Id.ToString(), user.Username, user.Email, user.FirstName, user.LastName, user.Role);
    }

    public string CreateToken(SiteUser user)
    {
        return CreateTokenInternal(user.Id.ToString(), user.Username, user.Email, user.FirstName, user.LastName, "User");
    }

    private string CreateTokenInternal(string userId, string username, string email, string firstName, string lastName, string role)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters"));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim("userId", userId),
            new Claim("username", username),
            new Claim("email", email),
            new Claim("firstName", firstName),
            new Claim("lastName", lastName),
            new Claim("role", role),
            new Claim(ClaimTypes.Role, role) // Standart role claim
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"] ?? "SubiletServer",
            audience: _configuration["Jwt:Audience"] ?? "SubiletClient",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
