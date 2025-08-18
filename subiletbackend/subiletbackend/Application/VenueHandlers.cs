using MediatR;
using SubiletBackend.Domain;
using SubiletBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SubiletBackend.Application
{
    public class GetVenuesQueryHandler : IRequestHandler<GetVenuesQuery, List<VenueResponse>>
    {
        private readonly AppDbContext _db;
        public GetVenuesQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<VenueResponse>> Handle(GetVenuesQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Venues.AsQueryable();
            if (!string.IsNullOrEmpty(request.Search))
                query = query.Where(v => v.Name.Contains(request.Search));
            return await query.Select(v => new VenueResponse
            {
                Id = v.Id,
                Name = v.Name,
                Address = v.Address,
                LayoutJson = v.LayoutJson
            }).ToListAsync();
        }
    }

    public class CreateVenueCommandHandler : IRequestHandler<CreateVenueCommand, VenueResponse>
    {
        private readonly AppDbContext _db;
        public CreateVenueCommandHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<VenueResponse> Handle(CreateVenueCommand request, CancellationToken cancellationToken)
        {
            var entity = new Venue
            {
                Name = request.Request.Name,
                Address = request.Request.Address,
                LayoutJson = request.Request.LayoutJson
            };
            _db.Venues.Add(entity);
            await _db.SaveChangesAsync();
            return new VenueResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Address = entity.Address,
                LayoutJson = entity.LayoutJson
            };
        }
    }
} 