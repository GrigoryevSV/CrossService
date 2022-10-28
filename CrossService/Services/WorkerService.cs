using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CrossService
{
    internal class WorkerService : BackgroundService
    {
        private readonly IBackGroundTaskQueue taskQueue;
        private readonly ILogger<WorkerService> logger;
        private readonly Settings settings;

        public WorkerService(IBackGroundTaskQueue taskQueue, ILogger<WorkerService> logger, Settings settings)
        {
            this.taskQueue = taskQueue;
            this.logger = logger;
            this.settings = settings;
        }
        protected override async Task ExecuteAsync(CancellationToken token)
        {
            var workersCount = settings.WorkersCount;
            var workers = Enumerable.Range(0, workersCount).Select(num => RunInstance(num, token));
            await Task.WhenAll(workers);
        }

        private async Task RunInstance(int num, CancellationToken token)
        {
            logger.LogInformation($"{this.settings.LogPrefix} instance {num} Process task started");

            while (!token.IsCancellationRequested)
            {
                var workitem = await taskQueue.DequeueAsunc(token);
                try
                {
                    logger.LogInformation($"{this.settings.LogPrefix} instance {num} Process task. Queue size {taskQueue.Size}");
                    await workitem(token);
                }
                catch (Exception ex)
                {
                    logger.LogError($"{ex}, {this.settings.LogPrefix} instance {num} Error");

                }
            }

            logger.LogInformation($"{this.settings.LogPrefix} instance {num} Process task finished");
        }
    }
}
