using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace SubiletBackend.Presentation
{
    [ApiController]
    [Route("api/sessions/{sessionId}/seats")]
    public class EventSessionSeatController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventSessionSeatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/sessions/{sessionId}/seats
        [HttpGet]
        public async Task<IActionResult> GetAll(int sessionId)
        {
            // Koltuk durumu listeleme handler'ı eklenecek
            return Ok();
        }
    }
} 