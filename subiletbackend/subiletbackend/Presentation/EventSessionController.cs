using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace SubiletBackend.Presentation
{
    [ApiController]
    [Route("api/events/{eventId}/sessions")]
    public class EventSessionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public EventSessionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/events/{eventId}/sessions
        [HttpGet]
        public async Task<IActionResult> GetAll(int eventId)
        {
            // Listeleme handler'ı eklenecek
            return Ok();
        }
    }

    [ApiController]
    [Route("api/admin/sessions")]
    public class AdminSessionController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminSessionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/admin/sessions
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] object request)
        {
            // Create handler'ı eklenecek
            return Ok();
        }

        // PUT /api/admin/sessions/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] object request)
        {
            // Update handler'ı eklenecek
            return Ok();
        }

        // DELETE /api/admin/sessions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Delete handler'ı eklenecek
            return Ok();
        }
    }
} 