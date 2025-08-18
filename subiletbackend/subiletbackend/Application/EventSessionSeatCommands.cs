using MediatR;
using System.Collections.Generic;

namespace SubiletBackend.Application
{
    public class GetEventSessionSeatsQuery : IRequest<List<EventSessionSeatResponse>>
    {
        public int SessionId { get; set; }
        public GetEventSessionSeatsQuery(int sessionId)
        {
            SessionId = sessionId;
        }
    }
} 