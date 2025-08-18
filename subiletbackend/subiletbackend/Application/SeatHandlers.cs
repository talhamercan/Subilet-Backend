using MediatR;
using SubiletBackend.Domain;
using SubiletBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SubiletBackend.Application
{
    public class GetSeatsQueryHandler : IRequestHandler<GetSeatsQuery, List<SeatResponse>>
    {
        private readonly AppDbContext _db;
        public GetSeatsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<SeatResponse>> Handle(GetSeatsQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Seats.AsQueryable();
            if (request.VenueId.HasValue)
                query = query.Where(s => s.VenueId == request.VenueId);
            if (!string.IsNullOrEmpty(request.Row))
                query = query.Where(s => s.Row == request.Row);
            if (!string.IsNullOrEmpty(request.Zone))
                query = query.Where(s => s.Zone == request.Zone);
            return await query.Select(s => new SeatResponse
            {
                Id = s.Id,
                VenueId = s.VenueId,
                Row = s.Row,
                Number = s.Number,
                Zone = s.Zone
            }).ToListAsync();
        }
    }

    public class BulkCreateSeatCommandHandler : IRequestHandler<BulkCreateSeatCommand, List<SeatResponse>>
    {
        private readonly AppDbContext _db;
        public BulkCreateSeatCommandHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<SeatResponse>> Handle(BulkCreateSeatCommand request, CancellationToken cancellationToken)
        {
            var entities = request.Requests.Select(r => new Seat
            {
                VenueId = r.VenueId,
                Row = r.Row,
                Number = r.Number,
                Zone = r.Zone
            }).ToList();
            _db.Seats.AddRange(entities);
            await _db.SaveChangesAsync();
            return entities.Select(s => new SeatResponse
            {
                Id = s.Id,
                VenueId = s.VenueId,
                Row = s.Row,
                Number = s.Number,
                Zone = s.Zone
            }).ToList();
        }
    }
} 