using System.Threading.Tasks;

namespace SubiletBackend.Application
{
    public class SeatReservationService
    {
        private readonly IRedisReservationService _redis;
        private readonly ISeatReservationNotifier _notifier;
        public SeatReservationService(IRedisReservationService redis, ISeatReservationNotifier notifier)
        {
            _redis = redis;
            _notifier = notifier;
        }

        public async Task ReserveSeatAsync(int sessionId, int seatId, int ttlSeconds = 600)
        {
            await _redis.SetSeatReservationAsync(sessionId, seatId, ttlSeconds);
            await _notifier.NotifySeatStatusAsync(sessionId, seatId, "Reserved");
        }

        public async Task ReleaseSeatAsync(int sessionId, int seatId)
        {
            await _redis.RemoveSeatReservationAsync(sessionId, seatId);
            await _notifier.NotifySeatStatusAsync(sessionId, seatId, "Available");
        }

        public async Task MarkSeatSoldAsync(int sessionId, int seatId)
        {
            await _redis.RemoveSeatReservationAsync(sessionId, seatId);
            await _notifier.NotifySeatStatusAsync(sessionId, seatId, "Sold");
        }
    }
} 