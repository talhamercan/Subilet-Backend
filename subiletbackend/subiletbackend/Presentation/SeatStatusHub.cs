using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SubiletBackend.Presentation
{
    public class SeatStatusHub : Hub
    {
        // Client'lara koltuk durumu güncellemesi gönder
        public async Task SendSeatStatusUpdate(int sessionId, int seatId, string status)
        {
            await Clients.All.SendAsync("SeatStatusUpdated", new
            {
                sessionId,
                seatId,
                status
            });
        }
    }
} 