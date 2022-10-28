using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossService.Extensions
{
    internal static class WindowsHostExtension
    {
        public static async Task RunService(this IHostBuilder hostBuilder)
        {
            if (!Environment.UserInteractive)
            {
                //await hostBuilder.RunAsServiceAsync();
            }
            else
            await hostBuilder.RunConsoleAsync();
        }

    }
}
