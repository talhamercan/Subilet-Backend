using Microsoft.AspNetCore.Mvc;
using MediatR;
using SubiletBackend.Application;
using System.Security.Claims;

namespace SubiletBackend.Presentation
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CartController(IMediator mediator) { _mediator = mediator; }

        // GET /api/cart
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new GetCartQuery(userId));
            return Ok(result);
        }

        // POST /api/cart/seats
        [HttpPost("seats")]
        public async Task<IActionResult> AddSeats([FromBody] CartItemAddRequest request)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new AddCartItemCommand(userId, request));
            return Ok(result);
        }

        // DELETE /api/cart/seats/{seatId}
        [HttpDelete("seats/{seatId}")]
        public async Task<IActionResult> RemoveSeat(int seatId)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new RemoveCartItemCommand(userId, seatId));
            return Ok(result);
        }

        private Guid GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdStr, out var guid) ? guid : Guid.Empty;
        }
    }

    [ApiController]
    [Route("api/checkout")]
    public class CheckoutController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CheckoutController(IMediator mediator) { _mediator = mediator; }

        // POST /api/checkout
        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            var userId = GetUserId();
            var result = await _mediator.Send(new CheckoutCommand(userId, request));
            return Ok(result);
        }

        private Guid GetUserId()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(userIdStr, out var guid) ? guid : Guid.Empty;
        }
    }
} 