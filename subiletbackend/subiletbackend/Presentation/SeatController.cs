using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace SubiletBackend.Presentation
{
    [ApiController]
    [Route("api/seats")]
    public class SeatController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SeatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/seats
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Listeleme handler'ı eklenecek
            return Ok();
        }

        // GET /api/seats/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Detay handler'ı eklenecek
            return Ok();
        }
    }

    [ApiController]
    [Route("api/admin/seats/bulk")]
    public class AdminSeatBulkController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminSeatBulkController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/admin/seats/bulk
        [HttpPost]
        public async Task<IActionResult> BulkCreate([FromBody] object request)
        {
            // Bulk create handler'ı eklenecek
            return Ok();
        }
    }
} 