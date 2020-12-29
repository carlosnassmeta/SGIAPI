using System.Collections.Generic;

namespace IMS.Application.Interface
{
    public interface IValidationDictionary
    {
        void AddError(string key, string errorMessage);
        bool HasErrors { get; }
        Dictionary<string, IList<string>> Errors { get; }
    }
}
