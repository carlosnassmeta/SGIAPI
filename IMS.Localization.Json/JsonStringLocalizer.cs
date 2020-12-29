using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using IMS.Localization.Json.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace IMS.Localization.Json
{
    public class JsonStringLocalizer : IStringLocalizer
    {
        private readonly ConcurrentDictionary<string, IEnumerable<KeyValuePair<string, string>>> _resourcesCache =
            new ConcurrentDictionary<string, IEnumerable<KeyValuePair<string, string>>>();
        private readonly string _resourcesPath;
        private readonly ILogger _logger;
        private readonly string _resourceName;

        private string _searchedLocation;

        public JsonStringLocalizer(
            ILogger logger,
            string resourcesPath,
            string resourceName = null)
        {
            _resourcesPath = resourcesPath ?? throw new ArgumentNullException(nameof(resourcesPath));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _resourceName = resourceName;
        }

        public LocalizedString this[string name]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));

                var value = GetStringSafely(name);

                return new LocalizedString(name, value ?? name, resourceNotFound: value == null, searchedLocation: _searchedLocation);
            }
        }

        public LocalizedString this[string name, params object[] arguments]
        {
            get
            {
                if (name == null) throw new ArgumentNullException(nameof(name));

                var format = GetStringSafely(name);
                var value = string.Format(format ?? name, arguments);

                return new LocalizedString(name, value, resourceNotFound: format == null, searchedLocation: _searchedLocation);
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) =>
            GetAllStrings(includeParentCultures, CultureInfo.CurrentUICulture);

        public IStringLocalizer WithCulture(CultureInfo culture) =>
            throw new NotSupportedException("WithCulture method is obsolete. Use CultureInfo.CurrentCulture instead.");

        protected IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures, CultureInfo culture)
        {
            if (culture == null) throw new ArgumentNullException(nameof(culture));

            var resourceNames = includeParentCultures
                ? GetAllStringsFromCultureHierarchy(culture)
                : GetAllResourceStrings(culture);

            foreach (var name in resourceNames)
            {
                var value = GetStringSafely(name);
                yield return new LocalizedString(name, value ?? name, resourceNotFound: value == null, searchedLocation: _searchedLocation);
            }
        }

        protected virtual string GetStringSafely(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var culture = CultureInfo.CurrentUICulture;
            var value = GetStringValue(culture, name, _resourceName);

            if (value == null)
                value = GetStringValue(culture, name, resourceName: null);

            return value;
        }

        private IEnumerable<string> GetAllStringsFromCultureHierarchy(CultureInfo startingCulture)
        {
            var currentCulture = startingCulture;
            var resourceNames = new HashSet<string>();

            while (currentCulture != currentCulture.Parent)
            {
                var cultureResourceNames = GetAllResourceStrings(currentCulture);

                if (cultureResourceNames != null)
                {
                    foreach (var resourceName in cultureResourceNames)
                    {
                        if (string.IsNullOrEmpty(resourceName)) continue;
                        resourceNames.Add(resourceName);
                    }
                }

                currentCulture = currentCulture.Parent;
            }

            return resourceNames;
        }

        private IEnumerable<string> GetAllResourceStrings(CultureInfo culture)
        {
            BuildResourcesCache(culture.Name, _resourceName, out string key);

            if (_resourcesCache.TryGetValue(key, out IEnumerable<KeyValuePair<string, string>> resources))
            {
                if (resources != null)
                {
                    foreach (var resource in resources)
                        yield return resource.Key;
                }
            }
            else
            {
                yield return null;
            }
        }

        private void BuildResourcesCache(string culture, string resourceName, out string key)
        {
            key = BuildDictionaryKey(culture, resourceName);

            _resourcesCache.GetOrAdd(key, _ =>
            {
                var resourceFile = string.IsNullOrEmpty(resourceName)
                   ? $"{culture}.json"
                   : $"{resourceName}.{culture}.json";

                _searchedLocation = Path.Combine(_resourcesPath, resourceFile);

                if (!File.Exists(_searchedLocation))
                    ConvertToFolderPathFromResourceFile(culture, resourceFile, out resourceFile);

                IEnumerable<KeyValuePair<string, string>> value = null;

                if (File.Exists(_searchedLocation))
                {
                    var builder = new ConfigurationBuilder()
                        .SetBasePath(_resourcesPath)
                        .AddJsonFile(resourceFile, optional: false, reloadOnChange: true);

                    var config = builder.Build();

                    value = config.AsEnumerable();
                }

                return value;
            });
        }

        private void ConvertToFolderPathFromResourceFile(string culture, string fileName, out string resourceFile)
        {
            resourceFile = fileName;
            var isConvertibleToFolderPath = fileName.Count(r => r == '.') > 1;

            if (!isConvertibleToFolderPath) return;

            var resourceFileWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var resourceFileWithoutCulture = resourceFileWithoutExtension.Substring(0, resourceFileWithoutExtension.LastIndexOf('.'));

            resourceFile = $"{resourceFileWithoutCulture.Replace('.', Path.DirectorySeparatorChar)}.{culture}.json";

            _searchedLocation = Path.Combine(_resourcesPath, resourceFile);
        }

        private string BuildDictionaryKey(string culture, string resourceName = null)
        {
            return string.IsNullOrEmpty(resourceName)
                ? $"{culture}"
                : $"{resourceName}.{culture}";
        }

        private string GetStringValue(CultureInfo culture, string stringName, string resourceName)
        {
            string value = null;

            while (culture != culture.Parent)
            {
                BuildResourcesCache(culture.Name, resourceName, out string key);

                if (_resourcesCache.TryGetValue(key, out IEnumerable<KeyValuePair<string, string>> resources))
                {
                    var resource = resources?.SingleOrDefault(s => s.Key == stringName);

                    value = resource?.Value ?? null;
                    _logger.SearchedLocation(stringName, _searchedLocation, culture);

                    if (value != null) break;

                    culture = culture.Parent;
                }
            }

            return value;
        }
    }
}
