using IMS.Application.Interface;
using System.Collections.Generic;

namespace IMS.Application.Service.Standard
{
    public class ModelStateService : IValidationDictionary
    {
        public bool HasErrors { get; private set; }

        public Dictionary<string, IList<string>> Errors { get; private set; }

        public ModelStateService()
        {
            HasErrors = false;
            Errors = new Dictionary<string, IList<string>>();
        }

        public void AddError(string key, string errorMessage)
        {
            HasErrors = true;

            var containsKey = Errors.ContainsKey(key);

            if (containsKey)
            {
                Errors[key].Add(errorMessage);
            }
            else
            {
                var values = new List<string> { errorMessage };

                Errors[key] = values;
            }
        }
    }
}
