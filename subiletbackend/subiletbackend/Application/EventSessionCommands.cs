using MediatR;
using System.Collections.Generic;

namespace SubiletBackend.Application
{
    public class CreateEventSessionCommand : IRequest<EventSessionResponse>
    {
        public EventSessionCreateRequest Request { get; set; }
        public CreateEventSessionCommand(EventSessionCreateRequest request)
        {
            Request = request;
        }
    }

    public class GetEventSessionsQuery : IRequest<List<EventSessionResponse>>
    {
        public int EventId { get; set; }
    }
} 