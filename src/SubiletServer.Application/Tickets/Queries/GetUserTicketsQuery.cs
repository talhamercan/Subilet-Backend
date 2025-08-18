using MediatR;
using SubiletServer.Domain.Abstractions;
using SubiletServer.Domain.Entities;

namespace SubiletServer.Application.Tickets.Queries
{
    public class GetUserTicketsQuery : IRequest<GetUserTicketsResponse>
    {
        public IdentityId UserId { get; set; } = default!;
    }

    public class GetUserTicketsResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<TicketDto> Tickets { get; set; } = new();
    }

    public class TicketDto
    {
        public IdentityId Id { get; set; } = default!;
        public IdentityId EventId { get; set; } = default!;
        public string EventName { get; set; } = string.Empty;
        public string EventLocation { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string TicketNumber { get; set; } = string.Empty;
        public TicketStatus Status { get; set; }
    }
} 