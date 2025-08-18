using MediatR;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;

namespace SubiletServer.Application.MusicEvents.Commands
{
    public class CreateMusicEventCommandHandler : IRequestHandler<CreateMusicEventCommand, Guid>
    {
        private readonly IMusicEventRepository _musicEventRepository;

        public CreateMusicEventCommandHandler(IMusicEventRepository musicEventRepository)
        {
            _musicEventRepository = musicEventRepository;
        }

        public async Task<Guid> Handle(CreateMusicEventCommand request, CancellationToken cancellationToken)
        {
            var musicEvent = new MusicEvent
            {
                ArtistName = request.ArtistName,
                Description = request.Description,
                Date = request.Date,
                Location = request.Location,
                Price = request.Price,
                Capacity = request.Capacity,
                ImageUrl = request.ImageUrl,
                Genre = request.Genre,
                Status = EventStatus.Active
            };

            var result = await _musicEventRepository.AddAsync(musicEvent);
            return result.Id.Value;
        }
    }
} 