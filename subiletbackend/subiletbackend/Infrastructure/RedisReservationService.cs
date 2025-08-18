using StackExchange.Redis;
using SubiletBackend.Application;
using System.Threading.Tasks;

namespace SubiletBackend.Infrastructure
{
    public class RedisReservationService : IRedisReservationService
    {
        private readonly IDatabase _db;
        public RedisReservationService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        private string GetKey(int sessionId, int seatId) => $"seat_reservation:{sessionId}:{seatId}";

        public async Task SetSeatReservationAsync(int sessionId, int seatId, int ttlSeconds)
        {
            await _db.StringSetAsync(GetKey(sessionId, seatId), "1", TimeSpan.FromSeconds(ttlSeconds));
        }

        public async Task<bool> IsSeatReservedAsync(int sessionId, int seatId)
        {
            return await _db.KeyExistsAsync(GetKey(sessionId, seatId));
        }

        public async Task RemoveSeatReservationAsync(int sessionId, int seatId)
        {
            await _db.KeyDeleteAsync(GetKey(sessionId, seatId));
        }
    }
} 