using Microsoft.Extensions.Logging;
using PlaygroundQuartz.Jobs;
using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

namespace PlaygroundQuartz
{
    public class SimpleExample
    {
        public virtual async Task Run()
        {
            ILoggerFactory logFactory = new LoggerFactory().AddConsole();
            ILogger logger = logFactory.CreateLogger<SimpleExample>();

            logger.LogInformation("------- Initializing ----------------------");

            // First we must get a reference to a scheduler
            ISchedulerFactory sf = new StdSchedulerFactory();
            IScheduler sched = await sf.GetScheduler();

            logger.LogInformation("------- Initialization Complete -----------");

            logger.LogInformation("------- Scheduling Job  -------------------");

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<HelloJob>()
                .WithIdentity("job1", "group1")
                .Build();

            //////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////
            // computer a time that is on the next round minute
            DateTimeOffset nextMinute = DateBuilder.EvenMinuteDate(DateTimeOffset.UtcNow);
            DateTimeOffset fiveSecondsLater = DateBuilder.NextGivenSecondDate(null, 10);

            // 1/3
            // Bir sonraki (çift sayı) dakika
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("trigger1", "group1")
            //    .StartAt(nextMinute)
            //    .Build();

            // 2/3
            // 3 saniyede bir, 5 kez
            ITrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .WithIdentity("trigger2", "group1")
                .StartAt(fiveSecondsLater)
                .WithSimpleSchedule(x => x.WithIntervalInSeconds(3).WithRepeatCount(5))
                .Build();

            ////// 3 / 3
            ////// 20 ile 59. saniyeler arasında her saniye
            //ITrigger trigger = TriggerBuilder.Create()
            //    .WithIdentity("trigger3", "group1")
            //    .WithCronSchedule("20-59 * * * * ?") //0/2
            //    .Build();

            //////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////
            //////////////////////////////////////////////////////////////

            // Tell quartz to schedule the job using our trigger
            await sched.ScheduleJob(job, trigger);

            // Start up the scheduler (nothing can actually run until the
            // scheduler has been started)
            await sched.Start();
            logger.LogInformation("------- Started Scheduler -----------------");

            // wait long enough so that the scheduler as an opportunity to
            // run the job!
            logger.LogInformation("------- Waiting 65 seconds... -------------");

            // wait 65 seconds to show jobs
            await Task.Delay(TimeSpan.FromSeconds(65));

            // shut down the scheduler
            logger.LogInformation("------- Shutting Down ---------------------");
            await sched.Shutdown(true);
            logger.LogInformation("------- Shutdown Complete -----------------");
        }
    }
}