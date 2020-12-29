using System.IO;
using System.Reflection;

namespace IMS.Localization.Json.Internal
{
    public static class PathHelpers
    {
        public static string GetApplicationRoot() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
