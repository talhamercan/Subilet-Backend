using MediatR;
using SubiletServer.Domain.Abstractions;

namespace SubiletServer.Application.Tickets.Commands
{
    public class PurchaseTicketCommand : IRequest<PurchaseTicketResponse>
    {
        public IdentityId EventId { get; set; } = default!;
        public IdentityId UserId { get; set; } = default!;
        public int Quantity { get; set; } = 1;
    }

    public class PurchaseTicketResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<TicketDto> Tickets { get; set; } = new();
        public decimal TotalAmount { get; set; }
    }

    public class TicketDto
    {
        public IdentityId Id { get; set; } = default!;
        public IdentityId EventId { get; set; } = default!;
        public IdentityId UserId { get; set; } = default!;
        public decimal Price { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string TicketNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
} 