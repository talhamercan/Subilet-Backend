using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SubiletServer.Application.Tickets.Commands;
using SubiletServer.Application.Tickets.Queries;
using SubiletServer.WebAPI.Models;
using SubiletServer.Domain.Abstractions;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;

namespace SubiletServer.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("purchase")]
        // [Authorize] // Geçici olarak kaldırıldı
        public async Task<IActionResult> PurchaseTicket([FromBody] PurchaseTicketRequest request)
        {
            try
            {
                // Debug: Frontend'den gelen veriyi logla
                Console.WriteLine("=== PURCHASE TICKET DEBUG ===");
                Console.WriteLine($"PurchaseTicket - Received EventId: '{request.EventId}'");
                Console.WriteLine($"PurchaseTicket - Received UserId: '{request.UserId}'");
                Console.WriteLine($"PurchaseTicket - Received Quantity: {request.Quantity}");
                Console.WriteLine("=============================");
                
                // String ID'leri IdentityId'ye çevir
                if (!Guid.TryParse(request.EventId, out var eventGuid))
                {
                    Console.WriteLine($"PurchaseTicket - Invalid EventId format: '{request.EventId}'");
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz event ID formatı."
                    });
                }

                if (!Guid.TryParse(request.UserId, out var userGuid))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz user ID formatı."
                    });
                }

                var command = new PurchaseTicketCommand
                {
                    EventId = new IdentityId(eventGuid),
                    UserId = new IdentityId(userGuid),
                    Quantity = request.Quantity
                };

                var result = await _mediator.Send(command);

                if (result.Success)
                {
                    return Ok(new ApiResponse<PurchaseTicketResponse>
                    {
                        Success = true,
                        Message = result.Message,
                        Data = result
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = result.Message
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Bilet satın alma işlemi sırasında bir hata oluştu.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpPost("test-purchase")]
        public async Task<IActionResult> TestPurchaseTicket()
        {
            try
            {
                // Test için yeni oluşturulan event ID ve gerçek admin user ID kullan
                var command = new PurchaseTicketCommand
                {
                    EventId = new IdentityId(Guid.Parse("503d1f1d-9f97-417e-8c4c-d1c3cc1f3336")), // Yeni test event ID
                    UserId = new IdentityId(Guid.Parse("bdf31412-8ee1-4d5e-82a7-bbe1854c6eda")), // Gerçek admin user ID
                    Quantity = 1
                };

                var result = await _mediator.Send(command);

                return Ok(new ApiResponse<object>
                {
                    Success = result.Success,
                    Message = result.Message,
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Test bilet satın alma işlemi sırasında bir hata oluştu.",
                    Errors = new List<string> { ex.Message, ex.StackTrace ?? "" }
                });
            }
        }

        [HttpPost("create-test-event")]
        public async Task<IActionResult> CreateTestEvent()
        {
            try
            {
                var @event = new Event
                {
                    Name = "Test Event",
                    Description = "Bilet test etkinliği",
                    Date = DateTime.Now.AddDays(30),
                    Location = "Test Lokasyon",
                    Price = 50.00m,
                    Capacity = 10,
                    SoldTickets = 0,
                    ImageUrl = "https://example.com/test.jpg",
                    Category = EventCategory.Music,
                    Status = EventStatus.Active
                };

                // EventRepository'yi kullanarak event'i kaydet
                var eventRepository = HttpContext.RequestServices.GetRequiredService<IEventRepository>();
                var savedEvent = await eventRepository.AddAsync(@event);

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Test event oluşturuldu",
                    Data = new { EventId = savedEvent.Id, EventName = savedEvent.Name }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Test event oluşturulurken hata oluştu",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("user/{userId}")]
        // [Authorize] // Geçici olarak kaldırıldı
        public async Task<IActionResult> GetUserTickets(string userId)
        {
            try
            {
                // Debug için userId'yi logla
                Console.WriteLine($"Received userId: {userId}");
                
                // userId string olarak geliyor, IdentityId'ye çevir
                if (!Guid.TryParse(userId, out var userGuid))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = $"Geçersiz kullanıcı ID formatı: {userId}. Beklenen format: GUID"
                    });
                }

                var userIdentityId = new IdentityId(userGuid);

                var query = new GetUserTicketsQuery
                {
                    UserId = userIdentityId
                };

                var result = await _mediator.Send(query);

                if (result.Success)
                {
                    return Ok(new ApiResponse<GetUserTicketsResponse>
                    {
                        Success = true,
                        Message = result.Message,
                        Data = result
                    });
                }
                else
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = result.Message
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Kullanıcı biletleri getirilirken bir hata oluştu.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }

        [HttpGet("event/{eventId}")]
        public async Task<IActionResult> GetEventTickets(string eventId)
        {
            try
            {
                // eventId string olarak geliyor, IdentityId'ye çevir
                if (!Guid.TryParse(eventId, out var eventGuid))
                {
                    return BadRequest(new ApiResponse<object>
                    {
                        Success = false,
                        Message = "Geçersiz etkinlik ID formatı."
                    });
                }

                var eventIdentityId = new IdentityId(eventGuid);

                // Bu endpoint için query handler oluşturulacak
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Message = "Etkinlik biletleri getirildi.",
                    Data = new { EventId = eventIdentityId, Message = "Bu endpoint henüz implement edilmedi." }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object>
                {
                    Success = false,
                    Message = "Etkinlik biletleri getirilirken bir hata oluştu.",
                    Errors = new List<string> { ex.Message }
                });
            }
        }
    }

    public class PurchaseTicketRequest
    {
        public string EventId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int Quantity { get; set; } = 1;
    }
} 