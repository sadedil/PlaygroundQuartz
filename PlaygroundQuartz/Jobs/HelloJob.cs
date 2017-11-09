using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;

namespace PlaygroundQuartz.Jobs
{
    public class HelloJob : IJob
    {
        private static readonly ILogger log = new LoggerFactory().AddConsole().CreateLogger<HelloJob>();

        public virtual Task Execute(IJobExecutionContext context)
        {
            // Say Hello to the World and display the date/time
            log.LogWarning($"Hello World! - {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}