using MediatR;
using System.Collections.Generic;

namespace SubiletBackend.Application
{
    public class CreateVenueCommand : IRequest<VenueResponse>
    {
        public VenueCreateRequest Request { get; set; }
        public CreateVenueCommand(VenueCreateRequest request)
        {
            Request = request;
        }
    }

    public class GetVenuesQuery : IRequest<List<VenueResponse>>
    {
        public string? Search { get; set; }
    }
} 