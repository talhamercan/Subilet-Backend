using MediatR;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;

namespace SubiletServer.Application.Tickets.Commands
{
    public class PurchaseTicketCommandHandler : IRequestHandler<PurchaseTicketCommand, PurchaseTicketResponse>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IUserRepository _userRepository;

        public PurchaseTicketCommandHandler(
            ITicketRepository ticketRepository,
            IEventRepository eventRepository,
            IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _eventRepository = eventRepository;
            _userRepository = userRepository;
        }

        public async Task<PurchaseTicketResponse> Handle(PurchaseTicketCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine($"PurchaseTicketCommandHandler: EventId={request.EventId}, UserId={request.UserId}, Quantity={request.Quantity}");
                
                // Event'i kontrol et
                var @event = await _eventRepository.GetByIdAsync(request.EventId);
                Console.WriteLine($"Event found: {@event?.Name ?? "null"}");
                
                if (@event == null)
                {
                    return new PurchaseTicketResponse
                    {
                        Success = false,
                        Message = "Etkinlik bulunamadı."
                    };
                }

                // Kullanıcıyı kontrol et - IdentityId'yi Guid'ye çevir
                var user = await _userRepository.GetByIdAsync(request.UserId.Value);
                Console.WriteLine($"User found: {user?.Username ?? "null"}");
                
                if (user == null)
                {
                    return new PurchaseTicketResponse
                    {
                        Success = false,
                        Message = "Kullanıcı bulunamadı."
                    };
                }

            // Etkinlik durumunu kontrol et
            if (@event.Status != EventStatus.Active)
            {
                return new PurchaseTicketResponse
                {
                    Success = false,
                    Message = "Bu etkinlik için bilet satışı aktif değil."
                };
            }

            // Yeterli bilet var mı kontrol et
            if (@event.AvailableTickets < request.Quantity)
            {
                return new PurchaseTicketResponse
                {
                    Success = false,
                    Message = $"Yeterli bilet yok. Mevcut: {@event.AvailableTickets}, İstenen: {request.Quantity}"
                };
            }

            var tickets = new List<Ticket>();
            var totalAmount = @event.Price * request.Quantity;

            // Biletleri oluştur
            for (int i = 0; i < request.Quantity; i++)
            {
                var ticket = new Ticket
                {
                    EventId = request.EventId,
                    UserId = request.UserId,
                    Price = @event.Price,
                    PurchaseDate = DateTime.UtcNow,
                    Status = TicketStatus.Active,
                    TicketNumber = GenerateTicketNumber()
                };

                var savedTicket = await _ticketRepository.AddAsync(ticket);
                tickets.Add(savedTicket);
            }

            // Event'in satılan bilet sayısını güncelle
            @event.SoldTickets += request.Quantity;
            if (@event.IsSoldOut)
            {
                @event.Status = EventStatus.SoldOut;
            }
            await _eventRepository.UpdateAsync(@event);

                return new PurchaseTicketResponse
                {
                    Success = true,
                    Message = $"{request.Quantity} adet bilet başarıyla satın alındı.",
                    Tickets = tickets.Select(t => new TicketDto
                    {
                        Id = t.Id,
                        EventId = t.EventId,
                        UserId = t.UserId,
                        Price = t.Price,
                        PurchaseDate = t.PurchaseDate,
                        TicketNumber = t.TicketNumber,
                        Status = t.Status.ToString()
                    }).ToList(),
                    TotalAmount = totalAmount
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PurchaseTicketCommandHandler Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                return new PurchaseTicketResponse
                {
                    Success = false,
                    Message = $"Bilet satın alma işlemi sırasında hata: {ex.Message}"
                };
            }
        }

        private string GenerateTicketNumber()
        {
            return $"TKT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpper()}";
        }
    }
} 