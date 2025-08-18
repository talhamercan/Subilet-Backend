using MediatR;

namespace SubiletBackend.Application
{
    public class CreateEventCommand : IRequest<EventResponse>
    {
        public EventCreateRequest Request { get; set; }
        public CreateEventCommand(EventCreateRequest request)
        {
            Request = request;
        }
    }

    public class UpdateEventCommand : IRequest<EventResponse>
    {
        public EventUpdateRequest Request { get; set; }
        public UpdateEventCommand(EventUpdateRequest request)
        {
            Request = request;
        }
    }

    public class DeleteEventCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public DeleteEventCommand(int id)
        {
            Id = id;
        }
    }

    public class GetEventByIdQuery : IRequest<EventResponse?>
    {
        public int Id { get; set; }
        public GetEventByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetEventsQuery : IRequest<List<EventResponse>>
    {
        public string? Search { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
} 