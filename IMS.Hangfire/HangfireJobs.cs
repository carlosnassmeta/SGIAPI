using Hangfire;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace IMS.Hangfire
{
    public class HangfireJobs
    {
        public static void ExecuteJob<T>(Expression<Action<T>> methodToExecute)
        {
            var interfacePath = methodToExecute.Type
                    .ToString()
                    .Split('.')
                    .LastOrDefault();
            var methodPath = methodToExecute
                .ToString()
                .Split('.')
                .LastOrDefault();
            var interfaceName = Regex.Replace(interfacePath, "[^a-zA-Z0-9]", string.Empty);
            var methodName = Regex.Replace(methodPath, "[^a-zA-Z0-9]", string.Empty);

            if (string.IsNullOrEmpty(interfaceName))
                throw new ArgumentNullException("Can't resolve interface name, please use an interface as T");
            if (string.IsNullOrEmpty(methodName))
                throw new ArgumentNullException("Can't resolve method name, please use a valid method from an interface");

            var jobId = $"{interfaceName}.{methodName}";

            RecurringJob.Trigger(jobId);
        }
    }
}
