using MediatR;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;

namespace SubiletServer.Application.SportEvents.Commands
{
    public class CreateSportEventCommandHandler : IRequestHandler<CreateSportEventCommand, Guid>
    {
        private readonly ISportEventRepository _sportEventRepository;

        public CreateSportEventCommandHandler(ISportEventRepository sportEventRepository)
        {
            _sportEventRepository = sportEventRepository;
        }

        public async Task<Guid> Handle(CreateSportEventCommand request, CancellationToken cancellationToken)
        {
            var sportEvent = new SportEvent
            {
                Title = request.Title,
                Description = request.Description,
                Date = request.Date,
                Location = request.Location,
                Price = request.Price,
                Capacity = request.Capacity,
                ImageUrl = request.ImageUrl,
                Genre = request.Genre,
                Status = EventStatus.Active
            };

            var result = await _sportEventRepository.AddAsync(sportEvent);
            return result.Id.Value;
        }
    }
} 