namespace SubiletServer.WebAPI.Models
{
    public class UpdateUserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Role { get; set; }
    }

    public class UpdateSiteUserRequest
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? IsUserActive { get; set; }
        public string? IsEmailVerified { get; set; }
        public string? EmailVerifiedAt { get; set; }
    }

    public class UpdateUserStatusRequest
    {
        public bool IsActive { get; set; }
    }

    public class UpdateUserRoleRequest
    {
        public string Role { get; set; } = string.Empty;
    }
}
