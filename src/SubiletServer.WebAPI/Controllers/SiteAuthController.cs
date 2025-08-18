using Microsoft.AspNetCore.Mvc;
using SubiletServer.Application.Services;
using SubiletServer.Domain.Users;
using SubiletServer.Infrastructure.Repositories;
using GenericRepository;
using SubiletServer.Domain.Abstractions;

namespace SubiletServer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/site-auth")]
    public class SiteAuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;

        public SiteAuthController(
            IUserRepository userRepository,
            IJwtProvider jwtProvider,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("giris-yap")]
        public async Task<IActionResult> Login([FromBody] SiteLoginRequest request)
        {
            try
            {
                Console.WriteLine($"=== LOGIN ATTEMPT ===");
                Console.WriteLine($"Email: {request.Email}");
                Console.WriteLine($"Password: {request.Password}");
                
                // Email ile kullanıcıyı bul
                var user = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
                
                // Login history için IP ve User Agent bilgilerini al
                var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

                if (user == null)
                {
                    Console.WriteLine("❌ USER NOT FOUND");
                    return BadRequest(new { message = "E-posta veya şifre hatalı" });
                }

                Console.WriteLine($"✅ USER FOUND: {user.Email}");
                Console.WriteLine($"User ID: {user.Id.Value}");

                // Şifreyi doğrula
                var passwordValid = user.VerifyPasswordHash(request.Password);
                Console.WriteLine($"Password verification result: {passwordValid}");
                Console.WriteLine($"Stored Password Hash Length: {user.Password.PasswordHash?.Length ?? 0}");
                Console.WriteLine($"Stored Password Salt Length: {user.Password.PasswordSalt?.Length ?? 0}");
                Console.WriteLine($"Input Password: {request.Password}");
                
                if (!passwordValid)
                {
                    Console.WriteLine("❌ PASSWORD INVALID");
                    return BadRequest(new { message = "E-posta veya şifre hatalı" });
                }

                Console.WriteLine("✅ PASSWORD VALID");

                if (!user.IsActive)
                {
                    Console.WriteLine("❌ USER NOT ACTIVE");
                    return BadRequest(new { message = "Hesabınız aktif değil" });
                }

                Console.WriteLine("✅ USER IS ACTIVE");

                // JWT token oluştur
                var token = _jwtProvider.CreateToken(user);
                Console.WriteLine("✅ TOKEN CREATED");

                Console.WriteLine("✅ LOGIN SUCCESSFUL");

                return Ok(new
                {
                    token,
                    user = new
                    {
                        id = user.Id.Value,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        email = user.Email,
                        username = user.Username,
                        isEmailVerified = false // User entity'sinde IsEmailVerified yok
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ LOGIN ERROR: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Giriş işlemi sırasında bir hata oluştu" });
            }
        }

        [HttpPost("kayit-ol")]
        public async Task<IActionResult> Register([FromBody] SiteRegisterRequest request)
        {
            try
            {
                Console.WriteLine($"=== REGISTER ATTEMPT ===");
                Console.WriteLine($"FullName: {request.FullName}");
                Console.WriteLine($"Email: {request.Email}");
                
                // Şifreler eşleşiyor mu kontrol et
                if (request.Password != request.PasswordConfirm)
                {
                    Console.WriteLine("❌ PASSWORD MISMATCH");
                    return BadRequest(new { message = "Şifreler eşleşmiyor" });
                }

                // Email zaten kullanılıyor mu kontrol et
                var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
                Console.WriteLine($"Email exists check for '{request.Email}': {existingUser != null}");
                
                if (existingUser != null)
                {
                    Console.WriteLine("❌ EMAIL ALREADY EXISTS");
                    return BadRequest(new { message = "Bu e-posta adresi zaten kullanılıyor" });
                }

                // Username zaten kullanılıyor mu kontrol et
                var existingUsername = await _userRepository.FirstOrDefaultAsync(u => u.Username == request.Email); // Email'i username olarak kullanıyoruz
                Console.WriteLine($"Username exists check for '{request.Email}': {existingUsername != null}");
                
                if (existingUsername != null)
                {
                    Console.WriteLine("❌ USERNAME ALREADY EXISTS");
                    return BadRequest(new { message = "Bu e-posta adresi zaten kullanılıyor" });
                }

                // Ad Soyad'ı parçala
                var nameParts = request.FullName.Trim().Split(' ', 2);
                var firstName = nameParts[0];
                var lastName = nameParts.Length > 1 ? nameParts[1] : "";

                Console.WriteLine($"Creating User: {firstName} {lastName}");

                // Yeni kullanıcı oluştur
                var newUser = new User(
                    firstName: firstName,
                    lastName: lastName,
                    email: request.Email,
                    username: request.Email, // Email'i username olarak kullanıyoruz
                    password: request.Password
                );

                Console.WriteLine($"User created with ID: {newUser.Id}");

                _userRepository.Add(newUser);
                Console.WriteLine("User added to repository");

                await _unitOfWork.SaveChangesAsync();
                Console.WriteLine("Changes saved to database");

                // JWT token oluştur
                var token = _jwtProvider.CreateToken(newUser);

                Console.WriteLine("✅ REGISTRATION SUCCESSFUL");

                return Ok(new
                {
                    token,
                    user = new
                    {
                        id = newUser.Id.Value,
                        firstName = newUser.FirstName,
                        lastName = newUser.LastName,
                        email = newUser.Email,
                        username = newUser.Username,
                        isEmailVerified = false // User entity'sinde IsEmailVerified yok
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ REGISTRATION ERROR: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                return StatusCode(500, new { message = "Kayıt işlemi sırasında bir hata oluştu", error = ex.Message });
            }
        }

        [HttpGet("profilim")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                {
                    return Unauthorized();
                }

                var user = await _userRepository.GetByIdAsync(new IdentityId(userGuid));
                if (user == null)
                {
                    return Unauthorized();
                }

                return Ok(new
                {
                    id = user.Id.Value,
                    firstName = user.FirstName,
                    lastName = user.LastName,
                    email = user.Email,
                    username = user.Username,
                    isEmailVerified = false // User entity'sinde IsEmailVerified yok
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Kullanıcı bilgileri alınırken bir hata oluştu" });
            }
        }

        [HttpPut("profil-guncelle")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                {
                    return Unauthorized();
                }

                var user = await _userRepository.GetByIdAsync(new IdentityId(userGuid));
                if (user == null)
                {
                    return Unauthorized();
                }

                // Email değişikliği varsa, başka kullanıcı tarafından kullanılıyor mu kontrol et
                if (request.Email != user.Email)
                {
                    var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
                    if (existingUser != null)
                    {
                        return BadRequest(new { message = "Bu e-posta adresi zaten kullanılıyor" });
                    }
                }

                // Profili güncelle - User entity'sinde UpdateProfile metodu yok, reflection kullanacağız
                var firstNameProperty = typeof(User).GetProperty("FirstName");
                var lastNameProperty = typeof(User).GetProperty("LastName");
                var emailProperty = typeof(User).GetProperty("Email");

                if (firstNameProperty != null) firstNameProperty.SetValue(user, request.FirstName);
                if (lastNameProperty != null) lastNameProperty.SetValue(user, request.LastName);
                if (emailProperty != null) emailProperty.SetValue(user, request.Email);

                await _unitOfWork.SaveChangesAsync();

                return Ok(new
                {
                    message = "Profil başarıyla güncellendi",
                    user = new
                    {
                        id = user.Id.Value,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        email = user.Email,
                        username = user.Username,
                        isEmailVerified = false // User entity'sinde IsEmailVerified yok
                    }
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Profil güncellenirken bir hata oluştu" });
            }
        }

        [HttpPut("sifre-degistir")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
                {
                    return Unauthorized();
                }

                var user = await _userRepository.GetByIdAsync(new IdentityId(userGuid));
                if (user == null)
                {
                    return Unauthorized();
                }

                // Mevcut şifreyi doğrula
                if (!user.VerifyPasswordHash(request.CurrentPassword))
                {
                    return BadRequest(new { message = "Mevcut şifre hatalı" });
                }

                // Yeni şifreler eşleşiyor mu kontrol et
                if (request.NewPassword != request.NewPasswordConfirm)
                {
                    return BadRequest(new { message = "Yeni şifreler eşleşmiyor" });
                }

                // Şifreyi güncelle - User entity'sinde UpdatePassword metodu yok, reflection kullanacağız
                var passwordProperty = typeof(User).GetProperty("Password");
                if (passwordProperty != null)
                {
                    var newPassword = new Password(request.NewPassword);
                    passwordProperty.SetValue(user, newPassword);
                }

                await _unitOfWork.SaveChangesAsync();

                return Ok(new { message = "Şifre başarıyla güncellendi" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Şifre güncellenirken bir hata oluştu" });
            }
        }
    }

    public class SiteLoginRequest
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }

    public class SiteRegisterRequest
    {
        public string FullName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string PasswordConfirm { get; set; } = default!;
    }

    public class UpdateProfileRequest
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
    }

    public class UpdatePasswordRequest
    {
        public string CurrentPassword { get; set; } = default!;
        public string NewPassword { get; set; } = default!;
        public string NewPasswordConfirm { get; set; } = default!;
    }
} 