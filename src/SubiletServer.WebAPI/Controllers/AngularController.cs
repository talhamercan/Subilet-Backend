using Microsoft.AspNetCore.Mvc;
using SubiletServer.WebAPI.Models;
using SubiletServer.Domain.Entities;

namespace SubiletServer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/angular")]
    public class AngularController : ControllerBase
    {
        /// <summary>
        /// Angular uygulamasının bağlantı durumunu kontrol et
        /// </summary>
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(ApiResponse<object>.SuccessResponse(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"
            }, "Angular backend bağlantısı başarılı"));
        }

        /// <summary>
        /// Angular için enum değerlerini getir
        /// </summary>
        [HttpGet("enums")]
        public IActionResult GetEnums()
        {
            var enums = new
            {
                MusicGenres = Enum.GetValues(typeof(MusicGenre))
                    .Cast<MusicGenre>()
                    .Select(e => new { Value = (int)e, Name = e.ToString(), DisplayName = GetDisplayName(e) }),

                SportGenres = Enum.GetValues(typeof(SportGenre))
                    .Cast<SportGenre>()
                    .Select(e => new { Value = (int)e, Name = e.ToString(), DisplayName = GetDisplayName(e) }),

                StageGenres = Enum.GetValues(typeof(StageGenre))
                    .Cast<StageGenre>()
                    .Select(e => new { Value = (int)e, Name = e.ToString(), DisplayName = GetDisplayName(e) }),

                EventStatuses = Enum.GetValues(typeof(EventStatus))
                    .Cast<EventStatus>()
                    .Select(e => new { Value = (int)e, Name = e.ToString(), DisplayName = GetDisplayName(e) })
            };

            return Ok(ApiResponse<object>.SuccessResponse(enums, "Enum değerleri başarıyla getirildi"));
        }

        /// <summary>
        /// Angular için uygulama bilgilerini getir
        /// </summary>
        [HttpGet("app-info")]
        public IActionResult GetAppInfo()
        {
            var appInfo = new
            {
                Name = "SubiletServer",
                Version = "1.0.0",
                Description = "Etkinlik Yönetim Sistemi",
                ApiVersion = "v1",
                Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development",
                Timestamp = DateTime.UtcNow,
                Features = new[]
                {
                    "JWT Authentication",
                    "CQRS Pattern",
                    "Entity Framework Core",
                    "Swagger Documentation",
                    "Health Checks",
                    "Structured Logging"
                }
            };

            return Ok(ApiResponse<object>.SuccessResponse(appInfo, "Uygulama bilgileri getirildi"));
        }

        /// <summary>
        /// Angular için test endpoint'i
        /// </summary>
        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok(ApiResponse<string>.SuccessResponse("Angular backend bağlantısı çalışıyor!", "Test başarılı"));
        }

        /// <summary>
        /// Angular için error test endpoint'i
        /// </summary>
        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            return BadRequest(ApiResponse<object>.ErrorResponse("Bu bir test hatasıdır", new List<string> { "Test error 1", "Test error 2" }));
        }

        private string GetDisplayName(Enum value)
        {
            return value switch
            {
                MusicGenre.Pop => "Pop",
                MusicGenre.Rock => "Rock",
                MusicGenre.Jazz => "Jazz",
                MusicGenre.Classical => "Klasik",
                MusicGenre.Folk => "Folk",
                MusicGenre.Electronic => "Elektronik",
                MusicGenre.Other => "Diğer",

                SportGenre.Football => "Futbol",
                SportGenre.Basketball => "Basketbol",
                SportGenre.Tennis => "Tenis",
                SportGenre.Volleyball => "Voleybol",
                SportGenre.Swimming => "Yüzme",
                SportGenre.Athletics => "Atletizm",
                SportGenre.Other => "Diğer",

                StageGenre.Theatre => "Tiyatro",
                StageGenre.Dance => "Dans",
                StageGenre.Opera => "Opera",
                StageGenre.Ballet => "Bale",
                StageGenre.Musical => "Müzikal",
                StageGenre.Comedy => "Komedi",
                StageGenre.Other => "Diğer",

                EventStatus.Active => "Aktif",
                EventStatus.Inactive => "Pasif",
                EventStatus.Cancelled => "İptal Edildi",
                EventStatus.SoldOut => "Tükendi",

                _ => value.ToString()
            };
        }
    }
} 