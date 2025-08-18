using MediatR;
using System.Collections.Generic;

namespace SubiletBackend.Application
{
    public class CreateArtistCommand : IRequest<ArtistResponse>
    {
        public ArtistCreateRequest Request { get; set; }
        public CreateArtistCommand(ArtistCreateRequest request)
        {
            Request = request;
        }
    }

    public class UpdateArtistCommand : IRequest<ArtistResponse>
    {
        public ArtistUpdateRequest Request { get; set; }
        public UpdateArtistCommand(ArtistUpdateRequest request)
        {
            Request = request;
        }
    }

    public class DeleteArtistCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteArtistCommand(int id)
        {
            Id = id;
        }
    }

    public class GetArtistByIdQuery : IRequest<ArtistResponse?>
    {
        public int Id { get; set; }
        public GetArtistByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetArtistsQuery : IRequest<List<ArtistResponse>>
    {
        public string? Search { get; set; }
    }
} 