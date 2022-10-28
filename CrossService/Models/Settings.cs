using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrossService
{
    internal class Settings
    {
        private IConfiguration configuration;

        public Settings(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public int WorkersCount { get { return configuration.GetValue<int>("WorkersCount"); } }
        public int RunInterval { get { return configuration.GetValue<int>("RunInterval"); } }

        public string InstanceName { get { return configuration.GetValue<string>("InstanceName"); } }
        public string ResultPath { get { return configuration.GetValue<string>("ResultPath"); } }
        public string LogPrefix = "TaskSheduleService";
    }
}
