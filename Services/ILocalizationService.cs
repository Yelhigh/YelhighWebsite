using System.Collections.Generic;

namespace YelhighWebsite.Services;

public interface ILocalizationService
{
    IDictionary<string, string> GetTexts(string languageCode);
    IDictionary<string, string> TranslateTexts(string languageCode, IEnumerable<string> sourceTexts);
}


