using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace SubiletBackend.Presentation
{
    [ApiController]
    [Route("api/venues")]
    public class VenueController : ControllerBase
    {
        private readonly IMediator _mediator;
        public VenueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/venues
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Listeleme handler'ı eklenecek
            return Ok();
        }

        // GET /api/venues/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Detay handler'ı eklenecek
            return Ok();
        }
    }

    [ApiController]
    [Route("api/admin/venues")]
    public class AdminVenueController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminVenueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/admin/venues
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] object request)
        {
            // Create handler'ı eklenecek
            return Ok();
        }

        // PUT /api/admin/venues/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] object request)
        {
            // Update handler'ı eklenecek
            return Ok();
        }

        // DELETE /api/admin/venues/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Delete handler'ı eklenecek
            return Ok();
        }
    }
} 