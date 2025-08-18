using Microsoft.AspNetCore.Mvc;
using SubiletServer.Domain.Users;
using SubiletServer.Infrastructure.Repositories;
using GenericRepository;
using SubiletServer.Domain.Entities;
using SubiletServer.WebAPI.Models;
using SubiletServer.Domain.Abstractions;
using System.Reflection;

namespace SubiletServer.WebAPI.Controllers
{
    [Route("api/admin")]
    [ApiController]
    [Microsoft.AspNetCore.Authorization.Authorize(Roles = "Admin")] // Sadece Admin role'üne sahip kullanıcılar
    public class AdminController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMusicEventRepository _musicEventRepository;
        private readonly ISportEventRepository _sportEventRepository;
        private readonly IStageEventRepository _stageEventRepository;
        private readonly ISiteUserRepository _siteUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(
            IUserRepository userRepository,
            IMusicEventRepository musicEventRepository,
            ISportEventRepository sportEventRepository,
            IStageEventRepository stageEventRepository,
            ISiteUserRepository siteUserRepository,
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _musicEventRepository = musicEventRepository;
            _sportEventRepository = sportEventRepository;
            _stageEventRepository = stageEventRepository;
            _siteUserRepository = siteUserRepository;
            _unitOfWork = unitOfWork;
        }

        #region User Management Endpoints

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                // Admin kullanıcıları (User tablosu)
                var adminUsers = await _userRepository.GetAllAsync();
                var adminUserDtos = adminUsers.Select(u => new
                {
                    Id = u.Id.Value,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username,
                    Role = u.Role,
                    UserType = "Admin",
                    IsUserActive = true, // Admin kullanıcıları her zaman aktif
                    IsEmailVerified = true, // Admin kullanıcıları her zaman doğrulanmış
                    EmailVerifiedAt = (DateTime?)null, // Admin için null
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                });

                // Site kullanıcıları (SiteUser tablosu)
                var siteUsers = await _siteUserRepository.GetAllAsync();
                var siteUserDtos = siteUsers.Select(u => new
                {
                    Id = u.Id.Value,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username,
                    Role = "User",
                    UserType = "SiteUser",
                    IsUserActive = u.IsUserActive,
                    IsEmailVerified = u.IsEmailVerified,
                    EmailVerifiedAt = u.EmailVerifiedAt,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                });

                // Tüm kullanıcıları birleştir
                var allUsers = adminUserDtos.Concat(siteUserDtos).ToList();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Tüm kullanıcılar başarıyla getirildi",
                    Data = allUsers
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

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out var userId))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz kullanıcı ID formatı",
                        Errors = new List<string> { "ID GUID formatında olmalıdır" }
                    });
                }

                var user = await _userRepository.GetByIdAsync(new IdentityId(userId));
                if (user == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Kullanıcı bulunamadı",
                        Errors = new List<string> { "Belirtilen ID'ye sahip kullanıcı bulunamadı" }
                    });
                }

                var userDto = new
                {
                    Id = user.Id.Value,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Kullanıcı başarıyla getirildi",
                    Data = userDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcı getirilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UpdateUserRequest request)
        {
            try
            {
                if (!Guid.TryParse(id, out var userId))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz kullanıcı ID formatı",
                        Errors = new List<string> { "ID GUID formatında olmalıdır" }
                    });
                }

                var user = await _userRepository.GetByIdAsync(new IdentityId(userId));
                if (user == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Kullanıcı bulunamadı",
                        Errors = new List<string> { "Belirtilen ID'ye sahip kullanıcı bulunamadı" }
                    });
                }

                // Email kontrolü (eğer değiştiriliyorsa)
                if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
                {
                    var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
                    if (existingUser != null)
                    {
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Bu email adresi zaten kullanılıyor",
                            Errors = new List<string> { "Email adresi başka bir kullanıcı tarafından kullanılıyor" }
                        });
                    }
                }

                // Username kontrolü (eğer değiştiriliyorsa)
                if (!string.IsNullOrEmpty(request.Username) && request.Username != user.Username)
                {
                    var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Username == request.Username);
                    if (existingUser != null)
                    {
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Bu kullanıcı adı zaten kullanılıyor",
                            Errors = new List<string> { "Kullanıcı adı başka bir kullanıcı tarafından kullanılıyor" }
                        });
                    }
                }

                // User entity'sinde property'ler private set olduğu için reflection kullanmamız gerekiyor
                var firstNameProperty = typeof(User).GetProperty("FirstName");
                var lastNameProperty = typeof(User).GetProperty("LastName");
                var emailProperty = typeof(User).GetProperty("Email");
                var usernameProperty = typeof(User).GetProperty("Username");
                var passwordProperty = typeof(User).GetProperty("Password");
                var roleProperty = typeof(User).GetProperty("Role");

                if (!string.IsNullOrEmpty(request.FirstName))
                    firstNameProperty?.SetValue(user, request.FirstName);

                if (!string.IsNullOrEmpty(request.LastName))
                    lastNameProperty?.SetValue(user, request.LastName);

                if (!string.IsNullOrEmpty(request.Email))
                    emailProperty?.SetValue(user, request.Email);

                if (!string.IsNullOrEmpty(request.Username))
                    usernameProperty?.SetValue(user, request.Username);

                if (!string.IsNullOrEmpty(request.Password))
                {
                    var newPassword = new Password(request.Password);
                    passwordProperty?.SetValue(user, newPassword);
                }

                if (!string.IsNullOrEmpty(request.Role))
                    roleProperty?.SetValue(user, request.Role);

                await _unitOfWork.SaveChangesAsync();

                var updatedUserDto = new
                {
                    Id = user.Id.Value,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                };

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Kullanıcı başarıyla güncellendi",
                    Data = updatedUserDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcı güncellenirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out var userId))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz kullanıcı ID formatı",
                        Errors = new List<string> { "ID GUID formatında olmalıdır" }
                    });
                }

                var user = await _userRepository.GetByIdAsync(new IdentityId(userId));
                if (user == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Kullanıcı bulunamadı",
                        Errors = new List<string> { "Belirtilen ID'ye sahip kullanıcı bulunamadı" }
                    });
                }

                if (user.Username == "admin")
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Admin kullanıcısı silinemez",
                        Errors = new List<string> { "Admin kullanıcısı sistem tarafından korunmaktadır" }
                    });
                }

                _userRepository.Delete(user);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Kullanıcı başarıyla silindi",
                    Data = new { DeletedUserId = user.Id.Value }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcı silinirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("users/{id}/status")]
        public async Task<IActionResult> UpdateUserStatus(string id, [FromBody] UpdateUserStatusRequest request)
        {
            try
            {
                if (!Guid.TryParse(id, out var userId))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz kullanıcı ID formatı",
                        Errors = new List<string> { "ID GUID formatında olmalıdır" }
                    });
                }

                var user = await _userRepository.GetByIdAsync(new IdentityId(userId));
                if (user == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Kullanıcı bulunamadı",
                        Errors = new List<string> { "Belirtilen ID'ye sahip kullanıcı bulunamadı" }
                    });
                }

                if (user.Username == "admin")
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Admin kullanıcısının durumu değiştirilemez",
                        Errors = new List<string> { "Admin kullanıcısı sistem tarafından korunmaktadır" }
                    });
                }

                // User entity'sinde IsActive property'si yok, bu yüzden bu endpoint'i kaldıralım
                return BadRequest(new ApiResponse<object>
                {
                    Success = false,
                    Message = "Bu özellik henüz desteklenmiyor",
                    Errors = new List<string> { "User entity'sinde IsActive property'si bulunmuyor" }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcı durumu güncellenirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("users/{id}/role")]
        public async Task<IActionResult> UpdateUserRole(string id, [FromBody] UpdateUserRoleRequest request)
        {
            try
            {
                if (!Guid.TryParse(id, out var userId))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz kullanıcı ID formatı",
                        Errors = new List<string> { "ID GUID formatında olmalıdır" }
                    });
                }

                var user = await _userRepository.GetByIdAsync(new IdentityId(userId));
                if (user == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Kullanıcı bulunamadı",
                        Errors = new List<string> { "Belirtilen ID'ye sahip kullanıcı bulunamadı" }
                    });
                }

                if (user.Username == "admin")
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Admin kullanıcısının rolü değiştirilemez",
                        Errors = new List<string> { "Admin kullanıcısı sistem tarafından korunmaktadır" }
                    });
                }

                // User entity'sinde Role property'si var, güncelle
                var roleProperty = typeof(User).GetProperty("Role");
                if (roleProperty != null)
                {
                    roleProperty.SetValue(user, request.Role);
                }

                await _unitOfWork.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Kullanıcı rolü başarıyla güncellendi",
                    Data = new
                    {
                        UserId = user.Id.Value,
                        Username = user.Username,
                        Role = user.Role
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcı rolü güncellenirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion

        #region Generic CRUD Operations

        [HttpGet("entities/{entityType}")]
        public async Task<IActionResult> GetAllEntities(string entityType)
        {
            try
            {
                var entities = await GetEntitiesByType(entityType);
                if (entities == null)
                    return BadRequest(new { message = $"Geçersiz entity tipi: {entityType}" });

                return Ok(entities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"{entityType} listesi getirilirken hata oluştu", error = ex.Message });
            }
        }

        [HttpGet("entities/{entityType}/{id}")]
        public async Task<IActionResult> GetEntityById(string entityType, Guid id)
        {
            try
            {
                var entity = await GetEntityByIdAndType(entityType, id);
                if (entity == null)
                    return NotFound(new { message = $"{entityType} bulunamadı" });

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"{entityType} getirilirken hata oluştu", error = ex.Message });
            }
        }

        [HttpPost("entities/{entityType}")]
        public async Task<IActionResult> CreateEntity(string entityType, [FromBody] Dictionary<string, object> request)
        {
            try
            {
                var entity = await CreateEntityByType(entityType, request);
                if (entity == null)
                    return BadRequest(new { message = $"Geçersiz entity tipi: {entityType}" });

                await _unitOfWork.SaveChangesAsync();

                // Entity'nin Id property'sine reflection ile erişim
                var idProperty = entity.GetType().GetProperty("Id");
                var id = idProperty?.GetValue(entity);

                return CreatedAtAction(nameof(GetEntityById), new { entityType, id }, entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"{entityType} oluşturulurken hata oluştu", error = ex.Message });
            }
        }

        [HttpPut("entities/{entityType}/{id}")]
        public async Task<IActionResult> UpdateEntity(string entityType, Guid id, [FromBody] Dictionary<string, object> request)
        {
            try
            {
                var entity = await UpdateEntityByType(entityType, id, request);
                if (entity == null)
                    return NotFound(new { message = $"{entityType} bulunamadı" });

                await _unitOfWork.SaveChangesAsync();

                return Ok(entity);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"{entityType} güncellenirken hata oluştu", error = ex.Message });
            }
        }

        [HttpDelete("entities/{entityType}/{id}")]
        public async Task<IActionResult> DeleteEntity(string entityType, Guid id)
        {
            try
            {
                var success = await DeleteEntityByType(entityType, id);
                if (!success)
                    return NotFound(new { message = $"{entityType} bulunamadı" });

                await _unitOfWork.SaveChangesAsync();

                return Ok(new { message = $"{entityType} başarıyla silindi" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"{entityType} silinirken hata oluştu", error = ex.Message });
            }
        }

        [HttpGet("entities/{entityType}/schema")]
        public IActionResult GetEntitySchema(string entityType)
        {
            try
            {
                var schema = GetSchemaByType(entityType);
                if (schema == null)
                    return BadRequest(new { message = $"Geçersiz entity tipi: {entityType}" });

                return Ok(schema);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Schema getirilirken hata oluştu", error = ex.Message });
            }
        }

        #endregion

        #region Helper Methods

        private async Task<object?> GetEntitiesByType(string entityType)
        {
            return entityType.ToLower() switch
            {
                "users" => await _userRepository.GetAllAsync(),
                "musicevents" => await _musicEventRepository.GetAllAsync(),
                "sportevents" => await _sportEventRepository.GetAllAsync(),
                "stageevents" => await _stageEventRepository.GetAllAsync(),
                _ => null
            };
        }

        private async Task<object?> GetEntityByIdAndType(string entityType, Guid id)
        {
            return entityType.ToLower() switch
            {
                "users" => await _userRepository.GetByIdAsync(new IdentityId(id)),
                "musicevents" => await _musicEventRepository.GetByIdAsync(new IdentityId(id)),
                "sportevents" => await _sportEventRepository.GetByIdAsync(new IdentityId(id)),
                "stageevents" => await _stageEventRepository.GetByIdAsync(new IdentityId(id)),
                _ => null
            };
        }

        private async Task<object?> CreateEntityByType(string entityType, Dictionary<string, object> request)
        {
            return entityType.ToLower() switch
            {
                "users" => await CreateUser(request),
                "musicevents" => await CreateMusicEvent(request),
                "sportevents" => await CreateSportEvent(request),
                "stageevents" => await CreateStageEvent(request),
                _ => null
            };
        }

        private async Task<object?> UpdateEntityByType(string entityType, Guid id, Dictionary<string, object> request)
        {
            return entityType.ToLower() switch
            {
                "users" => await UpdateUser(new IdentityId(id), request),
                "musicevents" => await UpdateMusicEvent(new IdentityId(id), request),
                "sportevents" => await UpdateSportEvent(new IdentityId(id), request),
                "stageevents" => await UpdateStageEvent(new IdentityId(id), request),
                _ => null
            };
        }

        private async Task<bool> DeleteEntityByType(string entityType, Guid id)
        {
            return entityType.ToLower() switch
            {
                "users" => await DeleteUser(new IdentityId(id)),
                "musicevents" => await DeleteMusicEvent(new IdentityId(id)),
                "sportevents" => await DeleteSportEvent(new IdentityId(id)),
                "stageevents" => await DeleteStageEvent(new IdentityId(id)),
                _ => false
            };
        }

        private object? GetSchemaByType(string entityType)
        {
            return entityType.ToLower() switch
            {
                "users" => new
                {
                    EntityName = "User",
                    Properties = new[]
                    {
                        new { Name = "FirstName", Type = "string", Required = true },
                        new { Name = "LastName", Type = "string", Required = true },
                        new { Name = "Email", Type = "string", Required = true },
                        new { Name = "Username", Type = "string", Required = true },
                        new { Name = "Password", Type = "string", Required = true },
                        new { Name = "IsActive", Type = "bool", Required = false }
                    }
                },
                "musicevents" => new
                {
                    EntityName = "MusicEvent",
                    Properties = new[]
                    {
                        new { Name = "ArtistName", Type = "string", Required = true },
                        new { Name = "Description", Type = "string", Required = true },
                        new { Name = "Date", Type = "DateTime", Required = true },
                        new { Name = "Location", Type = "string", Required = true },
                        new { Name = "Price", Type = "decimal", Required = true },
                        new { Name = "Capacity", Type = "int", Required = true },
                        new { Name = "ImageUrl", Type = "string", Required = false },
                        new { Name = "Genre", Type = "MusicGenre", Required = true },
                        new { Name = "Status", Type = "EventStatus", Required = false }
                    }
                },
                "sportevents" => new
                {
                    EntityName = "SportEvent",
                    Properties = new[]
                    {
                        new { Name = "Title", Type = "string", Required = true },
                        new { Name = "Description", Type = "string", Required = true },
                        new { Name = "Date", Type = "DateTime", Required = true },
                        new { Name = "Location", Type = "string", Required = true },
                        new { Name = "Price", Type = "decimal", Required = true },
                        new { Name = "Capacity", Type = "int", Required = true },
                        new { Name = "ImageUrl", Type = "string", Required = false },
                        new { Name = "Genre", Type = "SportGenre", Required = true },
                        new { Name = "Status", Type = "EventStatus", Required = false }
                    }
                },
                "stageevents" => new
                {
                    EntityName = "StageEvent",
                    Properties = new[]
                    {
                        new { Name = "Title", Type = "string", Required = true },
                        new { Name = "Description", Type = "string", Required = true },
                        new { Name = "Date", Type = "DateTime", Required = true },
                        new { Name = "Location", Type = "string", Required = true },
                        new { Name = "Price", Type = "decimal", Required = true },
                        new { Name = "Capacity", Type = "int", Required = true },
                        new { Name = "ImageUrl", Type = "string", Required = false },
                        new { Name = "Genre", Type = "StageGenre", Required = true },
                        new { Name = "Status", Type = "EventStatus", Required = false }
                    }
                },
                _ => null
            };
        }

        #endregion

        #region Entity-Specific CRUD Methods

        private async Task<User?> CreateUser(Dictionary<string, object> request)
        {
            if (!request.ContainsKey("FirstName") || !request.ContainsKey("LastName") || 
                !request.ContainsKey("Email") || !request.ContainsKey("Username") || 
                !request.ContainsKey("Password"))
                return null;

            var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Username == request["Username"].ToString());
            if (existingUser != null)
                throw new InvalidOperationException("Bu kullanıcı adı zaten kullanılıyor");

            existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == request["Email"].ToString());
            if (existingUser != null)
                throw new InvalidOperationException("Bu email adresi zaten kullanılıyor");

            var newUser = new User(
                firstName: request["FirstName"].ToString()!,
                lastName: request["LastName"].ToString()!,
                email: request["Email"].ToString()!,
                username: request["Username"].ToString()!,
                password: request["Password"].ToString()!
            );

            _userRepository.Add(newUser);
            return newUser;
        }

        private async Task<User?> UpdateUser(IdentityId id, Dictionary<string, object> request)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return null;

            foreach (var kvp in request)
            {
                var property = typeof(User).GetProperty(kvp.Key);
                if (property != null && property.CanWrite)
                {
                    var value = Convert.ChangeType(kvp.Value, property.PropertyType);
                    property.SetValue(user, value);
                }
            }

            return user;
        }

        private async Task<bool> DeleteUser(IdentityId id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return false;

            if (user.Username == "admin")
                throw new InvalidOperationException("Admin kullanıcısı silinemez");

            _userRepository.Delete(user);
            return true;
        }

        private async Task<MusicEvent?> CreateMusicEvent(Dictionary<string, object> request)
        {
            if (!request.ContainsKey("ArtistName") || !request.ContainsKey("Description") || 
                !request.ContainsKey("Date") || !request.ContainsKey("Location") || 
                !request.ContainsKey("Price") || !request.ContainsKey("Capacity"))
                return null;

            var musicEvent = new MusicEvent
            {
                ArtistName = request["ArtistName"].ToString()!,
                Description = request["Description"].ToString()!,
                Date = DateTime.Parse(request["Date"].ToString()!),
                Location = request["Location"].ToString()!,
                Price = Convert.ToDecimal(request["Price"]),
                Capacity = Convert.ToInt32(request["Capacity"]),
                ImageUrl = request.ContainsKey("ImageUrl") ? request["ImageUrl"].ToString()! : string.Empty,
                Genre = request.ContainsKey("Genre") ? (MusicGenre)Convert.ToInt32(request["Genre"]) : MusicGenre.Other,
                Status = request.ContainsKey("Status") ? (EventStatus)Convert.ToInt32(request["Status"]) : EventStatus.Active
            };

            return await _musicEventRepository.AddAsync(musicEvent);
        }

        private async Task<MusicEvent?> UpdateMusicEvent(IdentityId id, Dictionary<string, object> request)
        {
            var musicEvent = await _musicEventRepository.GetByIdAsync(id);
            if (musicEvent == null)
                return null;

            foreach (var kvp in request)
            {
                var property = typeof(MusicEvent).GetProperty(kvp.Key);
                if (property != null && property.CanWrite)
                {
                    var value = Convert.ChangeType(kvp.Value, property.PropertyType);
                    property.SetValue(musicEvent, value);
                }
            }

            return await _musicEventRepository.UpdateAsync(musicEvent);
        }

        private async Task<bool> DeleteMusicEvent(IdentityId id)
        {
            var musicEvent = await _musicEventRepository.GetByIdAsync(id);
            if (musicEvent == null)
                return false;

            await _musicEventRepository.DeleteAsync(id);
            return true;
        }

        private async Task<SportEvent?> CreateSportEvent(Dictionary<string, object> request)
        {
            if (!request.ContainsKey("Title") || !request.ContainsKey("Description") || 
                !request.ContainsKey("Date") || !request.ContainsKey("Location") || 
                !request.ContainsKey("Price") || !request.ContainsKey("Capacity"))
                return null;

            var sportEvent = new SportEvent
            {
                Title = request["Title"].ToString()!,
                Description = request["Description"].ToString()!,
                Date = DateTime.Parse(request["Date"].ToString()!),
                Location = request["Location"].ToString()!,
                Price = Convert.ToDecimal(request["Price"]),
                Capacity = Convert.ToInt32(request["Capacity"]),
                ImageUrl = request.ContainsKey("ImageUrl") ? request["ImageUrl"].ToString()! : string.Empty,
                Genre = request.ContainsKey("Genre") ? (SportGenre)Convert.ToInt32(request["Genre"]) : SportGenre.Other,
                Status = request.ContainsKey("Status") ? (EventStatus)Convert.ToInt32(request["Status"]) : EventStatus.Active
            };

            return await _sportEventRepository.AddAsync(sportEvent);
        }

        private async Task<SportEvent?> UpdateSportEvent(IdentityId id, Dictionary<string, object> request)
        {
            var sportEvent = await _sportEventRepository.GetByIdAsync(id);
            if (sportEvent == null)
                return null;

            foreach (var kvp in request)
            {
                var property = typeof(SportEvent).GetProperty(kvp.Key);
                if (property != null && property.CanWrite)
                {
                    var value = Convert.ChangeType(kvp.Value, property.PropertyType);
                    property.SetValue(sportEvent, value);
                }
            }

            return await _sportEventRepository.UpdateAsync(sportEvent);
        }

        private async Task<bool> DeleteSportEvent(IdentityId id)
        {
            var sportEvent = await _sportEventRepository.GetByIdAsync(id);
            if (sportEvent == null)
                return false;

            await _sportEventRepository.DeleteAsync(id);
            return true;
        }

        private async Task<StageEvent?> CreateStageEvent(Dictionary<string, object> request)
        {
            if (!request.ContainsKey("Title") || !request.ContainsKey("Description") || 
                !request.ContainsKey("Date") || !request.ContainsKey("Location") || 
                !request.ContainsKey("Price") || !request.ContainsKey("Capacity"))
                return null;

            var stageEvent = new StageEvent
            {
                Title = request["Title"].ToString()!,
                Description = request["Description"].ToString()!,
                Date = DateTime.Parse(request["Date"].ToString()!),
                Location = request["Location"].ToString()!,
                Price = Convert.ToDecimal(request["Price"]),
                Capacity = Convert.ToInt32(request["Capacity"]),
                ImageUrl = request.ContainsKey("ImageUrl") ? request["ImageUrl"].ToString()! : string.Empty,
                Genre = request.ContainsKey("Genre") ? (StageGenre)Convert.ToInt32(request["Genre"]) : StageGenre.Other,
                Status = request.ContainsKey("Status") ? (EventStatus)Convert.ToInt32(request["Status"]) : EventStatus.Active
            };

            return await _stageEventRepository.AddAsync(stageEvent);
        }

        private async Task<StageEvent?> UpdateStageEvent(IdentityId id, Dictionary<string, object> request)
        {
            var stageEvent = await _stageEventRepository.GetByIdAsync(id);
            if (stageEvent == null)
                return null;

            foreach (var kvp in request)
            {
                var property = typeof(StageEvent).GetProperty(kvp.Key);
                if (property != null && property.CanWrite)
                {
                    var value = Convert.ChangeType(kvp.Value, property.PropertyType);
                    property.SetValue(stageEvent, value);
                }
            }

            return await _stageEventRepository.UpdateAsync(stageEvent);
        }

        private async Task<bool> DeleteStageEvent(IdentityId id)
        {
            var stageEvent = await _stageEventRepository.GetByIdAsync(id);
            if (stageEvent == null)
                return false;

            await _stageEventRepository.DeleteAsync(id);
            return true;
        }

        #endregion

        #region Dashboard

        [HttpGet("debug/admin-user")]
        public async Task<IActionResult> GetAdminUserInfo()
        {
            try
            {
                var adminUser = await _userRepository.GetByUsernameAsync("admin");
                if (adminUser == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Admin kullanıcısı bulunamadı"
                    });
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Admin kullanıcı bilgileri",
                    Data = new
                    {
                        Id = adminUser.Id,
                        Username = adminUser.Username,
                        Email = adminUser.Email,
                        FirstName = adminUser.FirstName,
                        LastName = adminUser.LastName,
                        Role = adminUser.Role,
                        IsActive = adminUser.IsActive
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = $"Hata: {ex.Message}"
                });
            }
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats()
        {
            try
            {
                var allUsers = await _userRepository.GetAllAsync();
                var allMusicEvents = await _musicEventRepository.GetAllAsync();
                var allSportEvents = await _sportEventRepository.GetAllAsync();
                var allStageEvents = await _stageEventRepository.GetAllAsync();

                var stats = new
                {
                    Users = new
                    {
                        Total = allUsers.Count(),
                        Active = allUsers.Count(u => u.IsActive),
                        Inactive = allUsers.Count(u => !u.IsActive)
                    },
                    Events = new
                    {
                        MusicEvents = allMusicEvents.Count(),
                        SportEvents = allSportEvents.Count(),
                        StageEvents = allStageEvents.Count(),
                        Total = allMusicEvents.Count() + allSportEvents.Count() + allStageEvents.Count()
                    }
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Dashboard istatistikleri getirilirken hata oluştu", error = ex.Message });
            }
        }

        #endregion

        #region Enum Values

        [HttpGet("enums/{enumType}")]
        public IActionResult GetEnumValues(string enumType)
        {
            try
            {
                var enumValues = enumType.ToLower() switch
                {
                    "musicgenre" => Enum.GetValues(typeof(MusicGenre)).Cast<MusicGenre>().Select(e => new { Value = (int)e, Name = e.ToString() }),
                    "sportgenre" => Enum.GetValues(typeof(SportGenre)).Cast<SportGenre>().Select(e => new { Value = (int)e, Name = e.ToString() }),
                    "stagegenre" => Enum.GetValues(typeof(StageGenre)).Cast<StageGenre>().Select(e => new { Value = (int)e, Name = e.ToString() }),
                    "eventstatus" => Enum.GetValues(typeof(EventStatus)).Cast<EventStatus>().Select(e => new { Value = (int)e, Name = e.ToString() }),
                    _ => null
                };

                if (enumValues == null)
                    return BadRequest(new { message = $"Geçersiz enum tipi: {enumType}" });

                return Ok(enumValues);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"Enum değerleri getirilirken hata oluştu", error = ex.Message });
            }
        }

        [HttpGet("enums")]
        public IActionResult GetAllEnums()
        {
            try
            {
                var allEnums = new
                {
                    MusicGenre = Enum.GetValues(typeof(MusicGenre)).Cast<MusicGenre>().Select(e => new { Value = (int)e, Name = e.ToString() }),
                    SportGenre = Enum.GetValues(typeof(SportGenre)).Cast<SportGenre>().Select(e => new { Value = (int)e, Name = e.ToString() }),
                    StageGenre = Enum.GetValues(typeof(StageGenre)).Cast<StageGenre>().Select(e => new { Value = (int)e, Name = e.ToString() }),
                    EventStatus = Enum.GetValues(typeof(EventStatus)).Cast<EventStatus>().Select(e => new { Value = (int)e, Name = e.ToString() })
                };

                return Ok(allEnums);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Enum değerleri getirilirken hata oluştu", error = ex.Message });
            }
        }

        #endregion

        #region Site User Management Endpoints

        [HttpGet("siteusers")]
        public async Task<IActionResult> GetAllSiteUsers()
        {
            try
            {
                var siteUsers = await _siteUserRepository.GetAllAsync();
                var siteUserDtos = siteUsers.Select(u => new
                {
                    Id = u.Id.Value,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Username = u.Username,
                    IsUserActive = u.IsUserActive,
                    IsEmailVerified = u.IsEmailVerified,
                    EmailVerifiedAt = u.EmailVerifiedAt,
                    CreatedAt = u.CreatedAt,
                    UpdatedAt = u.UpdatedAt
                });

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Site kullanıcıları başarıyla getirildi",
                    Data = siteUserDtos
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Site kullanıcıları getirilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("siteusers/{id}")]
        public async Task<IActionResult> GetSiteUserById(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out var userId))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz kullanıcı ID formatı",
                        Errors = new List<string> { "ID GUID formatında olmalıdır" }
                    });
                }

                var siteUser = await _siteUserRepository.GetByIdAsync(userId);
                if (siteUser == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Site kullanıcısı bulunamadı",
                        Errors = new List<string> { "Belirtilen ID'ye sahip site kullanıcısı bulunamadı" }
                    });
                }

                var siteUserDto = new
                {
                    Id = siteUser.Id.Value,
                    FirstName = siteUser.FirstName,
                    LastName = siteUser.LastName,
                    Email = siteUser.Email,
                    Username = siteUser.Username,
                    IsUserActive = siteUser.IsUserActive,
                    IsEmailVerified = siteUser.IsEmailVerified,
                    EmailVerifiedAt = siteUser.EmailVerifiedAt,
                    CreatedAt = siteUser.CreatedAt,
                    UpdatedAt = siteUser.UpdatedAt
                };

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Site kullanıcısı başarıyla getirildi",
                    Data = siteUserDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Site kullanıcısı getirilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPut("siteusers/{id}")]
        public async Task<IActionResult> UpdateSiteUser(string id, [FromBody] UpdateSiteUserRequest request)
        {
            try
            {
                if (!Guid.TryParse(id, out var userId))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz kullanıcı ID formatı",
                        Errors = new List<string> { "ID GUID formatında olmalıdır" }
                    });
                }

                var siteUser = await _siteUserRepository.GetByIdAsync(userId);
                if (siteUser == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Site kullanıcısı bulunamadı",
                        Errors = new List<string> { "Belirtilen ID'ye sahip site kullanıcısı bulunamadı" }
                    });
                }

                // Email kontrolü (eğer değiştiriliyorsa)
                if (!string.IsNullOrEmpty(request.Email) && request.Email != siteUser.Email)
                {
                    var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
                    if (existingUser != null)
                    {
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Bu email adresi zaten kullanılıyor",
                            Errors = new List<string> { "Email adresi başka bir kullanıcı tarafından kullanılıyor" }
                        });
                    }
                }

                // Username kontrolü (eğer değiştiriliyorsa)
                if (!string.IsNullOrEmpty(request.Username) && request.Username != siteUser.Username)
                {
                    var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Username == request.Username);
                    if (existingUser != null)
                    {
                        return BadRequest(new ApiResponse<object>
                        {
                            Success = false,
                            Message = "Bu kullanıcı adı zaten kullanılıyor",
                            Errors = new List<string> { "Kullanıcı adı başka bir kullanıcı tarafından kullanılıyor" }
                        });
                    }
                }

                // SiteUser entity'sinde property'ler private set olduğu için reflection kullanmamız gerekiyor
                var firstNameProperty = typeof(SiteUser).GetProperty("FirstName");
                var lastNameProperty = typeof(SiteUser).GetProperty("LastName");
                var emailProperty = typeof(SiteUser).GetProperty("Email");
                var usernameProperty = typeof(SiteUser).GetProperty("Username");
                var passwordProperty = typeof(SiteUser).GetProperty("Password");
                var isUserActiveProperty = typeof(SiteUser).GetProperty("IsUserActive");
                var isEmailVerifiedProperty = typeof(SiteUser).GetProperty("IsEmailVerified");
                var emailVerifiedAtProperty = typeof(SiteUser).GetProperty("EmailVerifiedAt");

                if (!string.IsNullOrEmpty(request.FirstName))
                    firstNameProperty?.SetValue(siteUser, request.FirstName);

                if (!string.IsNullOrEmpty(request.LastName))
                    lastNameProperty?.SetValue(siteUser, request.LastName);

                if (!string.IsNullOrEmpty(request.Email))
                    emailProperty?.SetValue(siteUser, request.Email);

                if (!string.IsNullOrEmpty(request.Username))
                    usernameProperty?.SetValue(siteUser, request.Username);

                if (!string.IsNullOrEmpty(request.Password))
                {
                    var newPassword = new Password(request.Password);
                    passwordProperty?.SetValue(siteUser, newPassword);
                }

                if (!string.IsNullOrEmpty(request.IsUserActive))
                    isUserActiveProperty?.SetValue(siteUser, bool.Parse(request.IsUserActive));

                if (!string.IsNullOrEmpty(request.IsEmailVerified))
                    isEmailVerifiedProperty?.SetValue(siteUser, bool.Parse(request.IsEmailVerified));

                if (!string.IsNullOrEmpty(request.EmailVerifiedAt))
                    emailVerifiedAtProperty?.SetValue(siteUser, DateTime.Parse(request.EmailVerifiedAt));

                await _unitOfWork.SaveChangesAsync();

                var updatedSiteUserDto = new
                {
                    Id = siteUser.Id.Value,
                    FirstName = siteUser.FirstName,
                    LastName = siteUser.LastName,
                    Email = siteUser.Email,
                    Username = siteUser.Username,
                    IsUserActive = siteUser.IsUserActive,
                    IsEmailVerified = siteUser.IsEmailVerified,
                    EmailVerifiedAt = siteUser.EmailVerifiedAt,
                    CreatedAt = siteUser.CreatedAt,
                    UpdatedAt = siteUser.UpdatedAt
                };

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Site kullanıcı başarıyla güncellendi",
                    Data = updatedSiteUserDto
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Site kullanıcı güncellenirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpDelete("siteusers/{id}")]
        public async Task<IActionResult> DeleteSiteUser(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out var userId))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz kullanıcı ID formatı",
                        Errors = new List<string> { "ID GUID formatında olmalıdır" }
                    });
                }

                var siteUser = await _siteUserRepository.GetByIdAsync(userId);
                if (siteUser == null)
                {
                    return NotFound(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Site kullanıcısı bulunamadı",
                        Errors = new List<string> { "Belirtilen ID'ye sahip site kullanıcısı bulunamadı" }
                    });
                }

                _siteUserRepository.Delete(siteUser);
                await _unitOfWork.SaveChangesAsync();

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Site kullanıcı başarıyla silindi",
                    Data = new { DeletedUserId = siteUser.Id.Value }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Site kullanıcı silinirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        #endregion
    }
} 