using MediatR;
using SubiletBackend.Domain;
using SubiletBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SubiletBackend.Application
{
    public class GetEventSessionSeatsQueryHandler : IRequestHandler<GetEventSessionSeatsQuery, List<EventSessionSeatResponse>>
    {
        private readonly AppDbContext _db;
        public GetEventSessionSeatsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<EventSessionSeatResponse>> Handle(GetEventSessionSeatsQuery request, CancellationToken cancellationToken)
        {
            var query = from ess in _db.EventSessionSeats
                        join seat in _db.Seats on ess.SeatId equals seat.Id
                        where ess.SessionId == request.SessionId
                        select new EventSessionSeatResponse
                        {
                            SessionId = ess.SessionId,
                            SeatId = ess.SeatId,
                            Row = seat.Row,
                            Number = seat.Number,
                            Zone = seat.Zone,
                            IsReserved = ess.IsReserved,
                            IsSold = ess.IsSold
                        };
            return await query.ToListAsync();
        }
    }
} 