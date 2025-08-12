using Microsoft.AspNetCore.Mvc;
using SubiletServer.Domain.Users;
using SubiletServer.Infrastructure.Repositories;
using SubiletServer.Infrastructure.Services;
using GenericRepository;
using SubiletServer.Application.Services;
using SubiletServer.WebAPI.Models;

namespace SubiletServer.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(
            IUserRepository userRepository, 
            IJwtProvider jwtProvider, 
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz veri",
                        Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                    });

                var user = await _userRepository.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (user == null)
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Kullanıcı adı veya şifre hatalı"
                    });

                if (!user.VerifyPasswordHash(request.Password))
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Kullanıcı adı veya şifre hatalı"
                    });

                if (!user.IsActive)
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Hesabınız aktif değil"
                    });

                var token = _jwtProvider.CreateToken(user);

                return Ok(new ApiResponse<LoginResponse>
                {
                    Success = true,
                    Message = "Giriş başarılı",
                    Data = new LoginResponse
                    {
                        Token = token,
                        User = new UserDto
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Email = user.Email,
                            Username = user.Username,
                            IsActive = user.IsActive
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Giriş yapılırken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                Console.WriteLine($"=== REGISTER START ===");
                Console.WriteLine($"Register endpoint called with: {request.FirstName} {request.LastName}, {request.Email}, {request.Username}");
                
                if (!ModelState.IsValid)
                {
                    Console.WriteLine("ModelState is invalid");
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    Console.WriteLine($"Validation errors: {string.Join(", ", errors)}");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz veri",
                        Errors = errors
                    });
                }

                Console.WriteLine("Checking username existence...");
                // Kullanıcı adı kontrolü
                var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (existingUser != null)
                {
                    Console.WriteLine($"Username '{request.Username}' already exists");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Bu kullanıcı adı zaten kullanılıyor"
                    });
                }
                Console.WriteLine($"Username '{request.Username}' is available");

                Console.WriteLine("Checking email existence...");
                // Email kontrolü
                existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
                if (existingUser != null)
                {
                    Console.WriteLine($"Email '{request.Email}' already exists");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Bu email adresi zaten kullanılıyor"
                    });
                }
                Console.WriteLine($"Email '{request.Email}' is available");

                Console.WriteLine("Creating new User...");
                var newUser = new User(
                    firstName: request.FirstName,
                    lastName: request.LastName,
                    email: request.Email,
                    username: request.Username,
                    password: request.Password
                );
                Console.WriteLine($"User created with ID: {newUser.Id}");

                Console.WriteLine("Adding user to repository...");
                _userRepository.Add(newUser);
                Console.WriteLine("User added to repository");
                
                Console.WriteLine("Saving changes to database...");
                var saveResult = await _unitOfWork.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync completed. Result: {saveResult}");
                Console.WriteLine("User saved successfully!");

                Console.WriteLine("Creating JWT token...");
                var token = _jwtProvider.CreateToken(newUser);
                Console.WriteLine("JWT token created");

                Console.WriteLine("=== REGISTER SUCCESS ===");
                return CreatedAtAction(nameof(Login), new { }, new ApiResponse<LoginResponse>
                {
                    Success = true,
                    Message = "Kayıt başarılı",
                    Data = new LoginResponse
                    {
                        Token = token,
                        User = new UserDto
                        {
                            Id = newUser.Id,
                            FirstName = newUser.FirstName,
                            LastName = newUser.LastName,
                            Email = newUser.Email,
                            Username = newUser.Username,
                            IsActive = newUser.IsActive
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"=== REGISTER ERROR ===");
                Console.WriteLine($"Error in Register: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                Console.WriteLine($"Inner exception: {ex.InnerException?.Message}");
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kayıt olurken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (userId == Guid.Empty)
                    return Unauthorized(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz token"
                    });

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Kullanıcı bulunamadı"
                    });

                return Ok(new ApiResponse<UserDto>
                {
                    Success = true,
                    Message = "Kullanıcı bilgileri getirildi",
                    Data = new UserDto
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                        Username = user.Username,
                        IsActive = user.IsActive,
                        CreatedAt = user.CreatedAt.DateTime
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcı bilgileri getirilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("all-users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                var userDtos = users.Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt.DateTime
                }).ToList();

                return Ok(new ApiResponse<List<UserDto>>
                {
                    Success = true,
                    Message = $"Toplam {userDtos.Count} kullanıcı bulundu",
                    Data = userDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcılar getirilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("check-tables")]
        public async Task<IActionResult> CheckTables()
        {
            try
            {
                // SiteUsers tablosundaki kayıtları kontrol et
                var siteUsers = await _userRepository.GetAllAsync();
                
                // Users tablosundaki kayıtları da kontrol et (eğer UserRepository varsa)
                var userRepository = HttpContext.RequestServices.GetService<IUserRepository>();
                var users = new List<User>();
                if (userRepository != null)
                {
                    users = (await userRepository.GetAllAsync()).ToList();
                }

                var result = new
                {
                    SiteUsersCount = siteUsers.Count(),
                    UsersCount = users.Count,
                    SiteUsers = siteUsers.Select(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.Username }).ToList(),
                    Users = users.Select(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.Username }).ToList()
                };

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Tablo durumları kontrol edildi",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Tablolar kontrol edilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("database-info")]
        public async Task<IActionResult> GetDatabaseInfo()
        {
            try
            {
                await Task.Delay(1); // Async işlem simülasyonu
                
                // Veritabanı bağlantı bilgilerini al
                var connectionString = HttpContext.RequestServices.GetService<IConfiguration>()?.GetConnectionString("SqlServer");
                
                var result = new
                {
                    ConnectionString = connectionString,
                    DatabaseName = "subilet",
                    ServerName = "SAMET\\SQLEXPRESS",
                    SiteUsersTableExists = true, // Bu kontrol edilecek
                    UsersTableExists = true // Bu kontrol edilecek
                };

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Veritabanı bilgileri",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Veritabanı bilgileri alınırken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("debug-users")]
        public async Task<IActionResult> DebugUsers()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                var userList = users.Select(u => new
                {
                    Id = u.Id.Value,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username,
                    IsActive = u.IsActive,
                    CreatedAt = u.CreatedAt
                }).ToList();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = $"Toplam {userList.Count} kullanıcı bulundu",
                    Data = userList
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcılar getirilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        private Guid GetCurrentUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
                return Guid.Empty;

            return userId;
        }
    }



    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = new();
    }

    public class UserDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 