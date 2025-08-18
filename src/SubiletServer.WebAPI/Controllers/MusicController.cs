using MediatR;
using Microsoft.AspNetCore.Mvc;
using SubiletServer.Application.MusicEvents.Commands;
using SubiletServer.Application.MusicEvents.Queries;
using SubiletServer.Domain.Entities;
using SubiletServer.WebAPI.Models;

namespace SubiletServer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MusicController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MusicController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMusicEvents()
        {
            try
            {
                var query = new GetMusicEventsQuery();
                var result = await _mediator.Send(query);
                
                return Ok(new ApiResponse<List<MusicEventDto>>
                {
                    Success = true,
                    Message = "Müzik etkinlikleri başarıyla getirildi",
                    Data = result.Select(e => new MusicEventDto
                    {
                        Id = e.Id,
                        ArtistName = e.ArtistName,
                        Description = e.Description,
                        Date = e.Date,
                        Location = e.Location,
                        Price = e.Price,
                        Capacity = e.Capacity,
                        ImageUrl = e.ImageUrl,
                        Genre = e.Genre,
                        Status = e.Status
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Müzik etkinlikleri getirilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("pop")]
        public async Task<IActionResult> GetPopMusicEvents()
        {
            try
            {
                var query = new GetMusicEventsQuery { Genre = MusicGenre.Pop };
                var result = await _mediator.Send(query);
                
                return Ok(new ApiResponse<List<MusicEventDto>>
                {
                    Success = true,
                    Message = "Pop müzik etkinlikleri başarıyla getirildi",
                    Data = result.Select(e => new MusicEventDto
                    {
                        Id = e.Id,
                        ArtistName = e.ArtistName,
                        Description = e.Description,
                        Date = e.Date,
                        Location = e.Location,
                        Price = e.Price,
                        Capacity = e.Capacity,
                        ImageUrl = e.ImageUrl,
                        Genre = e.Genre,
                        Status = e.Status
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Pop müzik etkinlikleri getirilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("rock")]
        public async Task<IActionResult> GetRockMusicEvents()
        {
            try
            {
                var query = new GetMusicEventsQuery { Genre = MusicGenre.Rock };
                var result = await _mediator.Send(query);
                
                return Ok(new ApiResponse<List<MusicEventDto>>
                {
                    Success = true,
                    Message = "Rock müzik etkinlikleri başarıyla getirildi",
                    Data = result.Select(e => new MusicEventDto
                    {
                        Id = e.Id,
                        ArtistName = e.ArtistName,
                        Description = e.Description,
                        Date = e.Date,
                        Location = e.Location,
                        Price = e.Price,
                        Capacity = e.Capacity,
                        ImageUrl = e.ImageUrl,
                        Genre = e.Genre,
                        Status = e.Status
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Rock müzik etkinlikleri getirilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("jazz")]
        public async Task<IActionResult> GetJazzMusicEvents()
        {
            try
            {
                var query = new GetMusicEventsQuery { Genre = MusicGenre.Jazz };
                var result = await _mediator.Send(query);
                
                return Ok(new ApiResponse<List<MusicEventDto>>
                {
                    Success = true,
                    Message = "Jazz müzik etkinlikleri başarıyla getirildi",
                    Data = result.Select(e => new MusicEventDto
                    {
                        Id = e.Id,
                        ArtistName = e.ArtistName,
                        Description = e.Description,
                        Date = e.Date,
                        Location = e.Location,
                        Price = e.Price,
                        Capacity = e.Capacity,
                        ImageUrl = e.ImageUrl,
                        Genre = e.Genre,
                        Status = e.Status
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Jazz müzik etkinlikleri getirilirken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMusicEvent([FromBody] CreateMusicEventCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(GetAllMusicEvents), new { id = result }, new ApiResponse<Guid>
                {
                    Success = true,
                    Message = "Müzik etkinliği başarıyla oluşturuldu",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Müzik etkinliği oluşturulurken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("seed")]
        public async Task<IActionResult> SeedMusicEvents()
        {
            try
            {
                var events = new List<CreateMusicEventCommand>
                {
                    new CreateMusicEventCommand
                    {
                        ArtistName = "Test Artist 1",
                        Description = "Test müzik etkinliği 1",
                        Date = DateTime.Now.AddDays(30),
                        Location = "Test Lokasyon 1",
                        Price = 100.00m,
                        Capacity = 100,
                        ImageUrl = "https://example.com/image1.jpg",
                        Genre = MusicGenre.Pop
                    },
                    new CreateMusicEventCommand
                    {
                        ArtistName = "Test Artist 2",
                        Description = "Test müzik etkinliği 2",
                        Date = DateTime.Now.AddDays(60),
                        Location = "Test Lokasyon 2",
                        Price = 150.00m,
                        Capacity = 50,
                        ImageUrl = "https://example.com/image2.jpg",
                        Genre = MusicGenre.Rock
                    }
                };

                var createdEvents = new List<Guid>();

                foreach (var @event in events)
                {
                    var result = await _mediator.Send(@event);
                    createdEvents.Add(result);
                }

                return Ok(new ApiResponse<List<Guid>>
                {
                    Success = true,
                    Message = $"{createdEvents.Count} adet test etkinliği oluşturuldu",
                    Data = createdEvents
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Test etkinlikleri oluşturulurken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("create-test-event")]
        public async Task<IActionResult> CreateTestEvent()
        {
            try
            {
                var command = new CreateMusicEventCommand
                {
                    ArtistName = "Test Artist",
                    Description = "Bilet test etkinliği",
                    Date = DateTime.Now.AddDays(30),
                    Location = "Test Lokasyon",
                    Price = 50.00m,
                    Capacity = 10,
                    ImageUrl = "https://example.com/test.jpg",
                    Genre = MusicGenre.Pop
                };

                var result = await _mediator.Send(command);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Test etkinliği oluşturuldu",
                    Data = new { EventId = result, EventName = command.ArtistName }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Test etkinliği oluşturulurken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }



    public class MusicEventDto
    {
        public Guid Id { get; set; }
        public string ArtistName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Capacity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public MusicGenre Genre { get; set; }
        public EventStatus Status { get; set; }
    }
} 