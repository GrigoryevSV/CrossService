using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrossService
{
    internal class TaskSheduleService : IHostedService, IDisposable
    {
        private Timer timer;
        private readonly IServiceProvider services;
        private readonly Settings settings;
        private readonly ILogger<TaskSheduleService> logger;
        private readonly object syncRoot = new object();
        private readonly  Random random = new Random();
        
        public TaskSheduleService(IServiceProvider services)
        {
            this.services = services;
            this.settings = services.GetRequiredService<Settings>();
            this.logger = services.GetRequiredService<ILogger<TaskSheduleService>>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var interval = this.settings?.RunInterval ?? 0;

            if (interval == 0)
            {
                logger.LogWarning("settings.RunInterval == 0, RunInterval sets to default 60sec");
                interval = 60;
            }

            timer = new Timer((a) => ProcessTask(), null, TimeSpan.Zero, TimeSpan.FromSeconds(this.settings.RunInterval));
            return Task.CompletedTask;
        }

        private void ProcessTask()
        {
            if (Monitor.TryEnter(syncRoot))
            {
                logger.LogInformation($"{this.settings.LogPrefix} Process task started");
                for (int i = 0; i < 20; i++)
                    DoWork();

                logger.LogInformation($"{this.settings.LogPrefix} Process task finished");
                Monitor.Exit(syncRoot); 
            }
            else 
            {
                logger.LogInformation($"{this.settings.LogPrefix} Process task currently in progress");            
            }
        }
        private void DoWork()
        {
            var number = random.Next(20);
            var processor = services.GetRequiredService<TaskProcessor>();
            var queue = services.GetRequiredService<IBackGroundTaskQueue>();
            queue.QueueBackGroundWorkItem((token) =>
               {
                   return processor.RunAsync(number, token);
               });
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}
