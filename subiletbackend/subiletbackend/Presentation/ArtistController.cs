using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace SubiletBackend.Presentation
{
    [ApiController]
    [Route("api/artists")]
    public class ArtistController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ArtistController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/artists
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Listeleme handler'ı eklenecek
            return Ok();
        }

        // GET /api/artists/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            // Detay handler'ı eklenecek
            return Ok();
        }
    }

    [ApiController]
    [Route("api/admin/artists")]
    public class AdminArtistController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AdminArtistController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/admin/artists
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] object request)
        {
            // Create handler'ı eklenecek
            return Ok();
        }

        // PUT /api/admin/artists/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] object request)
        {
            // Update handler'ı eklenecek
            return Ok();
        }

        // DELETE /api/admin/artists/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Delete handler'ı eklenecek
            return Ok();
        }
    }
} 