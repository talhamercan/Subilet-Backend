using Microsoft.AspNetCore.Mvc;
using MediatR;
using SubiletBackend.Application;

namespace SubiletBackend.Presentation
{
    [ApiController]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/events
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] DateTime? dateFrom, [FromQuery] DateTime? dateTo)
        {
            var result = await _mediator.Send(new GetEventsQuery { Search = search, DateFrom = dateFrom, DateTo = dateTo });
            return Ok(result);
        }
    }

    [ApiController]
    [Route("api/admin/events")]
    public class AdminEventController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminEventController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/admin/events
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EventCreateRequest request)
        {
            var result = await _mediator.Send(new CreateEventCommand(request));
            return Ok(result);
        }
    }
} 