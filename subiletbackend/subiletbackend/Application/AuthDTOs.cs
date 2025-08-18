namespace SubiletBackend.Application
{
    public class RegisterRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Name { get; set; } = null!;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
} 