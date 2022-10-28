using Microsoft.Extensions.Logging;
using System.Text;

namespace CrossService
{
    internal class TaskProcessor
    {
        private readonly ILogger<TaskProcessor> logger;
        private readonly Settings settings;

        public TaskProcessor(ILogger<TaskProcessor> logger, Settings settings)
        {
            this.logger = logger;
            this.settings = settings;
        }

        public async Task RunAsync(int number, CancellationToken token)
        { 
            token.ThrowIfCancellationRequested();
            Func<int, int> fibonacci = null;

            fibonacci = (num) =>
            {
                if (num < 2) return 1;
                else return fibonacci(num - 1) + fibonacci(num - 2);
            };

            var result = await Task.Run(async () =>
            {
                await Task.Delay(1000);
                return Enumerable.Range(0, number).Select(n => fibonacci(n));
            }, token);

            using (var writer = new StreamWriter(settings.ResultPath, true, Encoding.UTF8))
            {
                writer.Write(string.Concat(DateTime.Now.ToString() , " : ", String.Join(" ", result)));
            }

            logger.LogInformation($"{this.settings.LogPrefix} : Task finished. Result: {string.Join(" ", result)}");
        }
    }
}
