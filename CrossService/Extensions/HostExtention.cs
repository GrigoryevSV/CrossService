using Microsoft.Extensions.Hosting;

namespace CrossService
{
    internal static class HostExtention
    {
        public static Task RunService(this IHostBuilder hostBuilder)
        {
            return hostBuilder.RunConsoleAsync();
        }
    }
}
