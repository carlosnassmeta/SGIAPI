using Hangfire;

namespace IMS.Hangfire
{
    public class HangfireJobScheduler
    {
        private static string everydayAt7Am = "0 8 * * *";

        public static void ScheduleRecurringJobs()
        {
            RecurringJob.AddOrUpdate<IImportProcessService>(x => x.ProcessAsync(), everydayAt7Am);
        }
    }
}
