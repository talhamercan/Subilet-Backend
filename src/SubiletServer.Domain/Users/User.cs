using SubiletServer.Domain.Abstractions;

public sealed class User : Entity
{
    public User(string firstName,
          string lastName,
          string email,
          string username,
          string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Username = username;
        Password = new Password(password);
        Role = "User"; // Default role
    }

    // Admin kullanıcısı için constructor
    public User(string firstName,
          string lastName,
          string email,
          string username,
          string password,
          string role)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Username = username;
        Password = new Password(password);
        Role = role;
    }

    // EF Core için gerekli parametresiz constructor
    private User() { }

    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public string Username { get; private set; } = default!;
    public Password Password { get; private set; } = default!;
    public string Role { get; private set; } = "User"; // Default role

    public bool VerifyPasswordHash(string password)
    {
        return Password.VerifyPassword(password);
    }

    // Role değiştirme metodu
    public void SetRole(string role)
    {
        Role = role;
    }
}
