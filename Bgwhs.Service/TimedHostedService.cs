using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bgwhs.Service
{
    public class TimedHostedService:IHostedService, IDisposable
    {

        private readonly ILogger _logger;
        private Timer _timer;
        //private ConsumeApiService _consumeApiService;
        private IServiceProvider _services { get; }
        public TimedHostedService(ILogger<TimedHostedService> logger, /*ConsumeApiService consumeApiService*/IServiceProvider services)
        {
            _logger = logger;
            //_consumeApiService = consumeApiService;
            _services = services;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero,
                TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            //use consumeapi service
            _logger.LogInformation("Timed Background Service is working.");
            //await _consumeApiService.ConsumeApi();
            using (var scope = _services.CreateScope())
            {
                var scopedProcessingService =
                    scope.ServiceProvider
                        .GetRequiredService<IScopedProcessingService>();

                await scopedProcessingService.ConsumeApi();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Timed Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
