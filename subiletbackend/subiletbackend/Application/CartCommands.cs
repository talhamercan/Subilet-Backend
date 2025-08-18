using MediatR;
using System.Collections.Generic;

namespace SubiletBackend.Application
{
    public class AddCartItemCommand : IRequest<CartResponse>
    {
        public Guid UserId { get; set; }
        public CartItemAddRequest Request { get; set; }
        public AddCartItemCommand(Guid userId, CartItemAddRequest request)
        {
            UserId = userId;
            Request = request;
        }
    }

    public class RemoveCartItemCommand : IRequest<CartResponse>
    {
        public Guid UserId { get; set; }
        public int SeatId { get; set; }
        public RemoveCartItemCommand(Guid userId, int seatId)
        {
            UserId = userId;
            SeatId = seatId;
        }
    }

    public class GetCartQuery : IRequest<CartResponse>
    {
        public Guid UserId { get; set; }
        public GetCartQuery(Guid userId)
        {
            UserId = userId;
        }
    }

    public class CheckoutCommand : IRequest<CheckoutResponse>
    {
        public Guid UserId { get; set; }
        public CheckoutRequest Request { get; set; }
        public CheckoutCommand(Guid userId, CheckoutRequest request)
        {
            UserId = userId;
            Request = request;
        }
    }
} 