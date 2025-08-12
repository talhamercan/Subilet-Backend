using MediatR;
using SubiletServer.Application.MusicEvents.Queries;
using SubiletServer.Domain.Users;

namespace SubiletServer.Application.MusicEvents.Queries
{
    public class GetMusicEventsQueryHandler : IRequestHandler<GetMusicEventsQuery, IEnumerable<MusicEventDto>>
    {
        private readonly IMusicEventRepository _musicEventRepository;

        public GetMusicEventsQueryHandler(IMusicEventRepository musicEventRepository)
        {
            _musicEventRepository = musicEventRepository;
        }

        public async Task<IEnumerable<MusicEventDto>> Handle(GetMusicEventsQuery request, CancellationToken cancellationToken)
        {
            var musicEvents = request.Genre.HasValue
                ? await _musicEventRepository.GetByGenreAsync(request.Genre.Value)
                : await _musicEventRepository.GetAllAsync();

            return musicEvents.Select(e => new MusicEventDto
            {
                Id = e.Id.Value,
                ArtistName = e.ArtistName,
                Description = e.Description,
                Date = e.Date,
                Location = e.Location,
                Price = e.Price,
                Capacity = e.Capacity,
                ImageUrl = e.ImageUrl,
                Genre = e.Genre,
                Status = e.Status
            });
        }
    }
} 