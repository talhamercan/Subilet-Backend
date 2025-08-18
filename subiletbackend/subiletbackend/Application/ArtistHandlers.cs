using MediatR;
using SubiletBackend.Domain;
using SubiletBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SubiletBackend.Application
{
    public class GetArtistsQueryHandler : IRequestHandler<GetArtistsQuery, List<ArtistResponse>>
    {
        private readonly AppDbContext _db;
        public GetArtistsQueryHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ArtistResponse>> Handle(GetArtistsQuery request, CancellationToken cancellationToken)
        {
            var query = _db.Artists.AsQueryable();
            if (!string.IsNullOrEmpty(request.Search))
                query = query.Where(a => a.Name.Contains(request.Search));
            return await query.Select(a => new ArtistResponse
            {
                Id = a.Id,
                Name = a.Name,
                Bio = a.Bio
            }).ToListAsync();
        }
    }

    public class CreateArtistCommandHandler : IRequestHandler<CreateArtistCommand, ArtistResponse>
    {
        private readonly AppDbContext _db;
        public CreateArtistCommandHandler(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ArtistResponse> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
        {
            var entity = new Artist
            {
                Name = request.Request.Name,
                Bio = request.Request.Bio
            };
            _db.Artists.Add(entity);
            await _db.SaveChangesAsync();
            return new ArtistResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Bio = entity.Bio
            };
        }
    }
} 