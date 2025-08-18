using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using SubiletBackend.Application;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SubiletBackend.Infrastructure
{
    public class SeatReservationBackgroundService : BackgroundService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<SeatReservationBackgroundService> _logger;
        public SeatReservationBackgroundService(IConnectionMultiplexer redis, IServiceProvider serviceProvider, ILogger<SeatReservationBackgroundService> logger)
        {
            _redis = redis;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var db = _redis.GetDatabase();
            var server = _redis.GetServer(_redis.GetEndPoints().First());
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var notifier = scope.ServiceProvider.GetRequiredService<ISeatReservationNotifier>();
                    try
                    {
                        var keys = server.Keys(pattern: "seat_reservation:*", pageSize: 100);
                        foreach (var key in keys)
                        {
                            var ttl = await db.KeyTimeToLiveAsync(key);
                            if (ttl == null || ttl.Value.TotalSeconds <= 0)
                            {
                                // Anahtar süresi dolmuş, koltuğu Available yap
                                var keyString = key.ToString();
                                var parts = keyString.Split(':');
                                if (parts.Length == 3 && int.TryParse(parts[1], out int sessionId) && int.TryParse(parts[2], out int seatId))
                                {
                                    await notifier.NotifySeatStatusAsync(sessionId, seatId, "Available");
                                    await db.KeyDeleteAsync(key);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "SeatReservationBackgroundService error");
                    }
                }
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
            }
        }
    }
} 