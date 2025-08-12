using MediatR;
using SubiletServer.Domain.Entities;
using SubiletServer.Domain.Users;

namespace SubiletServer.Application.StageEvents.Commands
{
    public class CreateStageEventCommandHandler : IRequestHandler<CreateStageEventCommand, Guid>
    {
        private readonly IStageEventRepository _stageEventRepository;

        public CreateStageEventCommandHandler(IStageEventRepository stageEventRepository)
        {
            _stageEventRepository = stageEventRepository;
        }

        public async Task<Guid> Handle(CreateStageEventCommand request, CancellationToken cancellationToken)
        {
            var stageEvent = new StageEvent
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

            var result = await _stageEventRepository.AddAsync(stageEvent);
            return result.Id.Value;
        }
    }
} 