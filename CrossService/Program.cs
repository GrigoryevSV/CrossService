using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrossService 
{
    internal class Program 
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(configBuilder =>
                {
                    configBuilder.AddJsonFile("config.json");
                    configBuilder.AddCommandLine(args);
                })
                .ConfigureLogging((configLogging) =>
                {
                    configLogging.AddConsole();
                    configLogging.AddDebug();
                }
                )
                .ConfigureServices((services) =>
                {
                    services.AddHostedService<WorkerService>();
                    services.AddHostedService<TaskSheduleService>();
                    services.AddSingleton<IBackGroundTaskQueue, BackGroundTaskQueue>();
                    services.AddSingleton<Settings>();
                    services.AddSingleton<TaskProcessor>();
                }
                );

            await builder.RunService();
        }
    }
}