using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using Prometheus;
using Prometheus.SystemMetrics;

namespace PromethusDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.FromLogContext()
                .WriteTo.Console(outputTemplate: "{Timestamp:G} [{Level:u3}] [{SourceContext}] {Message} {NewLine:1}{Exception:1}")
                .CreateLogger();

            var server = new MetricServer(hostname: "localhost", port: 9090);
            
            server.Start();

            await new HostBuilder().ConfigureServices((hostContext, service) =>
                {
                    service.AddSystemMetrics();
                    
                    service.AddScoped<IHostedService, SampleService>();
                })
                .UseSerilog()
                .RunConsoleAsync();
        }
    }
}
