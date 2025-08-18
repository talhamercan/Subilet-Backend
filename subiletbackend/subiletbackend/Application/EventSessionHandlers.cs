using MediatR;
using SubiletBackend.Domain;
using SubiletBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SubiletBackend.Application
{
    public class GetEventSessionsQueryHandler : IRequestHandler<GetEventSessionsQuery, List<EventSessionResponse>>
    {
        private readonly AppDbContext _db;
        public GetEventSessionsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<EventSessionResponse>> Handle(GetEventSessionsQuery request, CancellationToken cancellationToken)
        {
            var query = _db.EventSessions.AsQueryable();
            query = query.Where(s => s.EventId == request.EventId);
            return await query.Select(s => new EventSessionResponse
            {
                Id = s.Id,
                EventId = s.EventId,
                DateTime = s.DateTime,
                TotalCapacity = s.TotalCapacity
            }).ToListAsync();
        }
    }

    public class CreateEventSessionCommandHandler : IRequestHandler<CreateEventSessionCommand, EventSessionResponse>
    {
        private readonly AppDbContext _db;
        public CreateEventSessionCommandHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<EventSessionResponse> Handle(CreateEventSessionCommand request, CancellationToken cancellationToken)
        {
            var entity = new EventSession
            {
                EventId = request.Request.EventId,
                DateTime = request.Request.DateTime,
                TotalCapacity = request.Request.TotalCapacity
            };
            _db.EventSessions.Add(entity);
            await _db.SaveChangesAsync();
            return new EventSessionResponse
            {
                Id = entity.Id,
                EventId = entity.EventId,
                DateTime = entity.DateTime,
                TotalCapacity = entity.TotalCapacity
            };
        }
    }
} 