using MediatR;
using SubiletBackend.Domain;
using SubiletBackend.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace SubiletBackend.Application
{
    public class GetCartQueryHandler : IRequestHandler<GetCartQuery, CartResponse>
    {
        private readonly AppDbContext _db;
        public GetCartQueryHandler(AppDbContext db) { _db = db; }
        public async Task<CartResponse> Handle(GetCartQuery request, CancellationToken cancellationToken)
        {
            var cart = await _db.Carts.Include(c => c.Id)
                .FirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (cart == null)
                return new CartResponse { CartId = 0, Items = new() };
            var items = await _db.CartItems.Where(i => i.CartId == cart.Id)
                .Select(i => new CartItemResponse
                {
                    CartItemId = i.Id,
                    SessionId = i.SessionId,
                    SeatId = i.SeatId,
                    Qty = i.Qty
                }).ToListAsync();
            return new CartResponse { CartId = cart.Id, Items = items };
        }
    }

    public class AddCartItemCommandHandler : IRequestHandler<AddCartItemCommand, CartResponse>
    {
        private readonly AppDbContext _db;
        private readonly SeatReservationService _seatReservationService;
        public AddCartItemCommandHandler(AppDbContext db, SeatReservationService seatReservationService) { _db = db; _seatReservationService = seatReservationService; }
        public async Task<CartResponse> Handle(AddCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (cart == null)
            {
                cart = new Cart { UserId = request.UserId };
                _db.Carts.Add(cart);
                await _db.SaveChangesAsync();
            }
            foreach (var seatId in request.Request.SeatIds)
            {
                var item = new CartItem
                {
                    CartId = cart.Id,
                    SessionId = request.Request.SessionId,
                    SeatId = seatId,
                    Qty = request.Request.Qty
                };
                _db.CartItems.Add(item);
                // Koltuğu rezerve et ve SignalR ile bildir
                await _seatReservationService.ReserveSeatAsync(request.Request.SessionId, seatId);
            }
            await _db.SaveChangesAsync();
            var items = await _db.CartItems.Where(i => i.CartId == cart.Id)
                .Select(i => new CartItemResponse
                {
                    CartItemId = i.Id,
                    SessionId = i.SessionId,
                    SeatId = i.SeatId,
                    Qty = i.Qty
                }).ToListAsync();
            return new CartResponse { CartId = cart.Id, Items = items };
        }
    }

    public class RemoveCartItemCommandHandler : IRequestHandler<RemoveCartItemCommand, CartResponse>
    {
        private readonly AppDbContext _db;
        public RemoveCartItemCommandHandler(AppDbContext db) { _db = db; }
        public async Task<CartResponse> Handle(RemoveCartItemCommand request, CancellationToken cancellationToken)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (cart == null)
                return new CartResponse { CartId = 0, Items = new() };
            var item = await _db.CartItems.FirstOrDefaultAsync(i => i.CartId == cart.Id && i.SeatId == request.SeatId);
            if (item != null)
            {
                _db.CartItems.Remove(item);
                await _db.SaveChangesAsync();
            }
            var items = await _db.CartItems.Where(i => i.CartId == cart.Id)
                .Select(i => new CartItemResponse
                {
                    CartItemId = i.Id,
                    SessionId = i.SessionId,
                    SeatId = i.SeatId,
                    Qty = i.Qty
                }).ToListAsync();
            return new CartResponse { CartId = cart.Id, Items = items };
        }
    }

    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, CheckoutResponse>
    {
        private readonly AppDbContext _db;
        private readonly SeatReservationService _seatReservationService;
        public CheckoutCommandHandler(AppDbContext db, SeatReservationService seatReservationService) { _db = db; _seatReservationService = seatReservationService; }
        public async Task<CheckoutResponse> Handle(CheckoutCommand request, CancellationToken cancellationToken)
        {
            var cart = await _db.Carts.FirstOrDefaultAsync(c => c.UserId == request.UserId);
            if (cart == null)
                throw new Exception("Sepet bulunamadı");
            var order = new Order
            {
                UserId = request.UserId,
                CartId = cart.Id,
                Status = "PendingPayment"
            };
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
            // Sepetteki tüm koltukları 'Sold' yap ve SignalR ile bildir
            var items = await _db.CartItems.Where(i => i.CartId == cart.Id).ToListAsync();
            foreach (var item in items)
            {
                await _seatReservationService.MarkSeatSoldAsync(item.SessionId, item.SeatId);
            }
            return new CheckoutResponse { OrderId = order.Id, Status = order.Status };
        }
    }
} 