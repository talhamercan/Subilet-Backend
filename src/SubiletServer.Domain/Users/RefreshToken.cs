using SubiletServer.Domain.Abstractions;

namespace SubiletServer.Domain.Users
{
    public sealed class RefreshToken : Entity
    {
        public RefreshToken(string token, DateTime expiresAt, Guid userId)
        {
            Token = token;
            ExpiresAt = expiresAt;
            UserId = userId;
            IsRevoked = false;
        }

        // EF Core için gerekli parametresiz constructor
        private RefreshToken() { }

        public string Token { get; private set; } = default!;
        public DateTime ExpiresAt { get; private set; }
        public Guid UserId { get; private set; }
        public bool IsRevoked { get; private set; }

        public void Revoke()
        {
            IsRevoked = true;
        }

        public bool IsExpired => DateTime.UtcNow > ExpiresAt;
        public bool IsTokenActive => !IsRevoked && !IsExpired;
    }
} 