using MediatR;
using SubiletBackend.Domain;
using SubiletBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SubiletBackend.Application
{
    public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, EventResponse>
    {
        private readonly AppDbContext _db;
        public CreateEventCommandHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<EventResponse> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var entity = new Event
            {
                Name = request.Request.Name,
                Description = request.Request.Description,
                ArtistId = request.Request.ArtistId,
                Category = request.Request.Category,
                ImageUrl = request.Request.ImageUrl
            };
            _db.Events.Add(entity);
            await _db.SaveChangesAsync();
            return new EventResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ArtistId = entity.ArtistId,
                Category = entity.Category,
                ImageUrl = entity.ImageUrl
            };
        }
    }

    public class GetEventsQueryHandler : IRequestHandler<GetEventsQuery, List<EventResponse>>
    {
        private readonly AppDbContext _db;
        public GetEventsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<EventResponse>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Events.AsQueryable();
            if (!string.IsNullOrEmpty(request.Search))
                query = query.Where(e => e.Name.Contains(request.Search));
            // Tarih filtreleri EventSession ile genişletilebilir
            return await query.Select(e => new EventResponse
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                ArtistId = e.ArtistId,
                Category = e.Category,
                ImageUrl = e.ImageUrl
            }).ToListAsync();
        }
    }
} 