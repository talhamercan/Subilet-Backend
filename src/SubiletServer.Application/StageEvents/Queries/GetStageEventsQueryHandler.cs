using MediatR;
using SubiletServer.Application.StageEvents.Queries;
using SubiletServer.Domain.Users;

namespace SubiletServer.Application.StageEvents.Queries
{
    public class GetStageEventsQueryHandler : IRequestHandler<GetStageEventsQuery, IEnumerable<StageEventDto>>
    {
        private readonly IStageEventRepository _stageEventRepository;

        public GetStageEventsQueryHandler(IStageEventRepository stageEventRepository)
        {
            _stageEventRepository = stageEventRepository;
        }

        public async Task<IEnumerable<StageEventDto>> Handle(GetStageEventsQuery request, CancellationToken cancellationToken)
        {
            var stageEvents = request.Genre.HasValue
                ? await _stageEventRepository.GetByGenreAsync(request.Genre.Value)
                : await _stageEventRepository.GetAllAsync();

            return stageEvents.Select(e => new StageEventDto
            {
                Id = e.Id.Value,
                Title = e.Title,
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