using MediatR;
using SubiletServer.Application.SportEvents.Queries;
using SubiletServer.Domain.Users;

namespace SubiletServer.Application.SportEvents.Queries
{
    public class GetSportEventsQueryHandler : IRequestHandler<GetSportEventsQuery, IEnumerable<SportEventDto>>
    {
        private readonly ISportEventRepository _sportEventRepository;

        public GetSportEventsQueryHandler(ISportEventRepository sportEventRepository)
        {
            _sportEventRepository = sportEventRepository;
        }

        public async Task<IEnumerable<SportEventDto>> Handle(GetSportEventsQuery request, CancellationToken cancellationToken)
        {
            var sportEvents = request.Genre.HasValue
                ? await _sportEventRepository.GetByGenreAsync(request.Genre.Value)
                : await _sportEventRepository.GetAllAsync();

            return sportEvents.Select(e => new SportEventDto
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