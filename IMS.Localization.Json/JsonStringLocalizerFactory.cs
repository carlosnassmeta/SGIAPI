using System;
using System.IO;
using System.Reflection;
using IMS.Localization.Json.Internal;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IMS.Localization.Json
{
    public class JsonStringLocalizerFactory : IStringLocalizerFactory
    {
        private readonly string _resourcesRelativePath;
        private readonly ILogger<JsonStringLocalizer> _logger;

        public JsonStringLocalizerFactory(IOptions<JsonLocalizationOptions> localizationOptions, ILogger<JsonStringLocalizer> logger)
        {
            if (localizationOptions == null) throw new ArgumentNullException(nameof(localizationOptions));

            _resourcesRelativePath = localizationOptions.Value.ResourcesPath ?? string.Empty;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IStringLocalizer Create(Type resourceSource)
        {
            if (resourceSource == null) throw new ArgumentNullException(nameof(resourceSource));

            var typeInfo = resourceSource.GetTypeInfo();
            var assembly = typeInfo.Assembly;
            var resourcesPath = Path.Combine(PathHelpers.GetApplicationRoot(), GetResourcePath(assembly));

            return CreateJsonStringLocalizer(resourcesPath, resourceName: typeInfo.Name);
        }

        public IStringLocalizer Create(string baseName, string location)
        {
            if (baseName == null) throw new ArgumentNullException(nameof(baseName));
            if (location == null) throw new ArgumentNullException(nameof(location));

            var resourcesPath = Path.Combine(PathHelpers.GetApplicationRoot(), _resourcesRelativePath);

            return CreateJsonStringLocalizer(resourcesPath, resourceName: baseName);
        }

        protected virtual JsonStringLocalizer CreateJsonStringLocalizer(string resourcesPath, string resourceName)
        {
            return new JsonStringLocalizer(_logger, resourcesPath, resourceName);
        }

        private string GetResourcePath(Assembly assembly)
        {
            var resourceLocationAttribute = assembly.GetCustomAttribute<ResourceLocationAttribute>();

            return resourceLocationAttribute == null
                ? _resourcesRelativePath
                : resourceLocationAttribute.ResourceLocation;
        }
    }
}
