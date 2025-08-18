using MediatR;
using System.Collections.Generic;

namespace SubiletBackend.Application
{
    public class CreateSeatCommand : IRequest<SeatResponse>
    {
        public SeatCreateRequest Request { get; set; }
        public CreateSeatCommand(SeatCreateRequest request)
        {
            Request = request;
        }
    }

    public class BulkCreateSeatCommand : IRequest<List<SeatResponse>>
    {
        public List<SeatCreateRequest> Requests { get; set; } = new();
    }

    public class GetSeatsQuery : IRequest<List<SeatResponse>>
    {
        public int? VenueId { get; set; }
        public string? Row { get; set; }
        public string? Zone { get; set; }
    }
} 