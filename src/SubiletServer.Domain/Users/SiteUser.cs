using SubiletServer.Domain.Abstractions;

namespace SubiletServer.Domain.Users
{
    public sealed class SiteUser : Entity
    {
        public SiteUser(string firstName, string lastName, string email, string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            Password = new Password(password);
            IsUserActive = true;
            IsEmailVerified = false;
        }

        // EF Core için gerekli parametresiz constructor
        private SiteUser() { }

        public string FirstName { get; private set; } = default!;
        public string LastName { get; private set; } = default!;
        public string Email { get; private set; } = default!;
        public string Username { get; private set; } = default!;
        public Password Password { get; private set; } = default!;
        public bool IsUserActive { get; private set; }
        public bool IsEmailVerified { get; private set; }
        public DateTime? EmailVerifiedAt { get; private set; }

        public bool VerifyPassword(string password)
        {
            return Password.VerifyPassword(password);
        }

        public void SetActive(bool isActive)
        {
            IsUserActive = isActive;
        }

        public void VerifyEmail()
        {
            IsEmailVerified = true;
            EmailVerifiedAt = DateTime.UtcNow;
        }

        // Profil güncelleme metodları
        public void UpdateProfile(string firstName, string lastName, string email)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public void UpdatePassword(string newPassword)
        {
            Password = new Password(newPassword);
        }

        public void UpdateUsername(string newUsername)
        {
            Username = newUsername;
        }
    }
} 