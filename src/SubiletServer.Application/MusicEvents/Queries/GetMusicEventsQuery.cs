using MediatR;
using SubiletServer.Domain.Entities;

namespace SubiletServer.Application.MusicEvents.Queries
{
    public record GetMusicEventsQuery : IRequest<IEnumerable<MusicEventDto>>
    {
        public MusicGenre? Genre { get; init; }
    }

    public record MusicEventDto
    {
        public Guid Id { get; init; }
        public string ArtistName { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public DateTime Date { get; init; }
        public string Location { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Capacity { get; init; }
        public string ImageUrl { get; init; } = string.Empty;
        public MusicGenre Genre { get; init; }
        public EventStatus Status { get; init; }
    }
} 