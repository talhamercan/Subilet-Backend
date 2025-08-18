using MediatR;
using Microsoft.EntityFrameworkCore;
using SubiletBackend.Application;
using SubiletBackend.Domain;
using SubiletBackend.Infrastructure;

namespace SubiletBackend.Application
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, AuthResponse>
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _authService;

        public RegisterUserCommandHandler(AppDbContext db, IAuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _db.Users.AnyAsync(u => u.Email == request.Request.Email))
                throw new Exception("Email zaten kayıtlı!");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Request.Email,
                Name = request.Request.Name,
                PasswordHash = _authService.HashPassword(request.Request.Password),
                Role = "User"
            };

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = _authService.GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                User = user
            };

            user.RefreshTokens.Add(refreshToken);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return new AuthResponse
            {
                UserId = user.Id,
                Token = _authService.GenerateJwtToken(user),
                RefreshToken = refreshToken.Token
            };
        }
    }

    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, AuthResponse>
    {
        private readonly AppDbContext _db;
        private readonly IAuthService _authService;

        public LoginUserCommandHandler(AppDbContext db, IAuthService authService)
        {
            _db = db;
            _authService = authService;
        }

        public async Task<AuthResponse> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _db.Users.Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == request.Request.Email);

            if (user == null || !_authService.VerifyPassword(user.PasswordHash, request.Request.Password))
                throw new Exception("Geçersiz email veya şifre!");

            if (user.IsBanned)
                throw new Exception("Kullanıcı banlı!");

            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = _authService.GenerateRefreshToken(),
                Expires = DateTime.UtcNow.AddDays(7),
                User = user
            };

            user.RefreshTokens.Add(refreshToken);
            await _db.SaveChangesAsync();

            return new AuthResponse
            {
                UserId = user.Id,
                Token = _authService.GenerateJwtToken(user),
                RefreshToken = refreshToken.Token
            };
        }
    }
} 