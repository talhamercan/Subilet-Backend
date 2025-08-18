using System.Threading.Tasks;

namespace SubiletBackend.Application
{
    public interface IRedisReservationService
    {
        Task SetSeatReservationAsync(int sessionId, int seatId, int ttlSeconds);
        Task<bool> IsSeatReservedAsync(int sessionId, int seatId);
        Task RemoveSeatReservationAsync(int sessionId, int seatId);
    }
} 