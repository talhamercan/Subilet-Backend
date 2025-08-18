using System.Threading.Tasks;

namespace SubiletBackend.Application
{
    public interface ISeatReservationNotifier
    {
        Task NotifySeatStatusAsync(int sessionId, int seatId, string status);
    }
} 