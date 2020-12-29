using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Linq;

namespace IMS.Localization.Json
{
    public static class StringLocalizerExtensions
    {
        public static IEnumerable<string> GetAllStringsValues(this IStringLocalizer stringLocalizer, bool includeParentCultures = true)
            => stringLocalizer.GetAllStrings(includeParentCultures).Select(x => x.Value).ToArray();

        public static string GetValue(this IStringLocalizer stringLocalizer, string name)
            => stringLocalizer[name].Value ?? null;

        public static string GetValue(this IStringLocalizer stringLocalizer, string name, params object[] args)
            => string.Format(stringLocalizer[name].Value ?? string.Empty, args);
    }
}
