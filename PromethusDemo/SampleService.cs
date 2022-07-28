using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prometheus;
using System.Threading;
using System.Threading.Tasks;

namespace PromethusDemo
{
    internal class SampleService : IHostedService
    {
        private readonly ILogger<SampleService> _logger;
        CancellationTokenSource cts;

        private readonly Counter counter = Metrics.CreateCounter("sampleapp_ticks_total", "running counter");


        public SampleService(ILogger<SampleService> logger)
        {
            _logger = logger;
            cts = new CancellationTokenSource();
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {


            Task.Run(() =>
            {
                while (true)
                {
                    _logger.LogInformation("Running");
                    counter.Inc();
                    Thread.Sleep(1000);
                }
            }, cts.Token);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            cts.Cancel();
            _logger.LogWarning("Stopping");
            return Task.CompletedTask;
        }
    }
}