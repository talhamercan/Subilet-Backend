using SubiletServer.Domain.Abstractions;

namespace SubiletServer.Domain.Users
{
    public sealed class LoginHistory : Entity
    {
        public LoginHistory(Guid userId, string ipAddress, string userAgent, bool isSuccessful)
        {
            UserId = userId;
            IpAddress = ipAddress;
            UserAgent = userAgent;
            IsSuccessful = isSuccessful;
        }

        // EF Core için gerekli parametresiz constructor
        private LoginHistory() { }

        public Guid UserId { get; private set; }
        public string IpAddress { get; private set; } = default!;
        public string UserAgent { get; private set; } = default!;
        public bool IsSuccessful { get; private set; }
    }
} 