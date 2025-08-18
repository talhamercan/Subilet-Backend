using MediatR;
using SubiletServer.Domain.Entities;

namespace SubiletServer.Application.MusicEvents.Commands
{
    public record CreateMusicEventCommand : IRequest<Guid>
    {
        public string ArtistName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime Date { get; init; }
        public string Location { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Capacity { get; init; }
        public string ImageUrl { get; init; } = string.Empty;
        public MusicGenre Genre { get; init; }
    }
} 