using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace IMS.Hangfire.Configuration
{
    public static class HangfireIoC
    {
        public static void AddHangfireServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config
                  .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                  .UseSimpleAssemblyNameTypeSerializer()
                  .UseRecommendedSerializerSettings()
                  .UseSqlServerStorage(configuration.GetConnectionString("SqlServer"), new SqlServerStorageOptions
                  {
                      SchemaName = "ims-hangfire",
                      CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                      SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                      QueuePollInterval = TimeSpan.Zero,
                      DisableGlobalLocks = true,
                      UseRecommendedIsolationLevel = true
                  });
            });
            services.AddHangfireServer();
        }

        public static void AddHangfire(this IApplicationBuilder app, IConfiguration configuration)
        {
            var showDashboard = configuration.GetValue<bool?>("Hangfire:ShowDashboard");
            var dashboardUrl = configuration.GetValue<string>("Hangfire:Url");

            if (showDashboard == null)
                throw new ArgumentNullException("Invalid hangfire showDashboard configurations, check appsettings.json");
            if (dashboardUrl == null)
                throw new ArgumentNullException("Invalid hangfire url, check appsettings.json");

            if (!dashboardUrl.StartsWith("/")) dashboardUrl = $"/{dashboardUrl}";

            app.UseHangfireDashboard(dashboardUrl, new DashboardOptions
            {
                IsReadOnlyFunc = context => !showDashboard.Value
            });

            app.UseHangfireServer();

            HangfireJobScheduler.ScheduleRecurringJobs();
        }
    }
}
