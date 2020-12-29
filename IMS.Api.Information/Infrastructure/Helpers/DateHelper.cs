using System;

namespace IMS.Api.Information.Infrastructure.Helpers
{
    public static class DateHelper
    {
        /// <returns>Date converted to seconds since (Jan 1, 1970, midnight UTC).</returns>
        public static long ToSecondsTimestamp(DateTime date) =>
            (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
    }
}
