using MediatR;
using SubiletServer.Domain.Entities;

namespace SubiletServer.Application.SportEvents.Commands
{
    public record CreateSportEventCommand : IRequest<Guid>
    {
        public string Title { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime Date { get; init; }
        public string Location { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Capacity { get; init; }
        public string ImageUrl { get; init; } = string.Empty;
        public SportGenre Genre { get; init; }
    }
} 