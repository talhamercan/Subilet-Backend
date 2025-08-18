using MediatR;
using Microsoft.EntityFrameworkCore;
using SubiletServer.Domain.Abstractions;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;

namespace SubiletServer.Application.Tickets.Queries
{
    public class GetUserTicketsQueryHandler : IRequestHandler<GetUserTicketsQuery, GetUserTicketsResponse>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IUserRepository _userRepository;

        public GetUserTicketsQueryHandler(
            ITicketRepository ticketRepository,
            IUserRepository userRepository)
        {
            _ticketRepository = ticketRepository;
            _userRepository = userRepository;
        }

        public async Task<GetUserTicketsResponse> Handle(GetUserTicketsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Kullanıcının var olup olmadığını kontrol et
                var user = await _userRepository.GetByIdAsync(request.UserId.Value);
                if (user == null)
                {
                    return new GetUserTicketsResponse
                    {
                        Success = false,
                        Message = "Kullanıcı bulunamadı."
                    };
                }

                // Kullanıcının biletlerini getir
                var tickets = await _ticketRepository.GetUserTicketsAsync(request.UserId);

                var ticketDtos = tickets.Select(ticket => new TicketDto
                {
                    Id = ticket.Id,
                    EventId = ticket.EventId,
                    EventName = ticket.Event.Name,
                    EventLocation = ticket.Event.Location,
                    EventDate = ticket.Event.Date,
                    Price = ticket.Price,
                    PurchaseDate = ticket.PurchaseDate,
                    TicketNumber = ticket.TicketNumber,
                    Status = ticket.Status
                }).ToList();

                return new GetUserTicketsResponse
                {
                    Success = true,
                    Message = $"{ticketDtos.Count} adet bilet bulundu.",
                    Tickets = ticketDtos
                };
            }
            catch (Exception ex)
            {
                return new GetUserTicketsResponse
                {
                    Success = false,
                    Message = $"Biletler getirilirken hata oluştu: {ex.Message}"
                };
            }
        }
    }
} 