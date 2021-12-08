using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TopGames.Core.Services;

namespace TopGames.Services
{
    public class BackgroundService : IHostedService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BackgroundService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        private Timer _timer = null!;

        public BackgroundService(IConfiguration configuration,
                                 ILogger<BackgroundService> logger,
                                 IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Service is running.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(_configuration.GetValue<int>("BackgroundService:Period")));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            var startTime = DateTime.Now;
            _logger.LogInformation($"Start time: {startTime:dd.MM.yyyy}");

            try
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var scraperService = scope.ServiceProvider.GetRequiredService<IScraperService>();
                    var gameService = scope.ServiceProvider.GetRequiredService<IGameService>();

                    var topGames = scraperService.GetTopGames();

                    await gameService.AddRangeAsync(topGames);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error received during BackgroundService-DoWork operation. Message: {ex.Message} - Detail: {ex.StackTrace}");
            }

            var endTime = DateTime.Now;
            _logger.LogInformation($"End time: {endTime:dd.MM.yyyy} - Duration: {(endTime - startTime).TotalSeconds}sec");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
