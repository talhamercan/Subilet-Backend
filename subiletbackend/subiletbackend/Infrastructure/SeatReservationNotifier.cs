using Microsoft.AspNetCore.SignalR;
using SubiletBackend.Application;
using SubiletBackend.Presentation;
using System.Threading.Tasks;

namespace SubiletBackend.Infrastructure
{
    public class SeatReservationNotifier : ISeatReservationNotifier
    {
        private readonly IHubContext<SeatStatusHub> _hubContext;
        public SeatReservationNotifier(IHubContext<SeatStatusHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifySeatStatusAsync(int sessionId, int seatId, string status)
        {
            await _hubContext.Clients.All.SendAsync("SeatStatusUpdated", new
            {
                sessionId,
                seatId,
                status
            });
        }
    }
} 