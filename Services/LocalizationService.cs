using System.Collections;
using System.Globalization;
using System.Resources;
using Microsoft.Extensions.Options;

namespace YelhighWebsite.Services;

public sealed class LocalizationService : ILocalizationService
{
    private readonly HashSet<string> _globalEnglishTerms;
    private static readonly ResourceManager ResourceManager = new ResourceManager(
        typeof(LocalizationService).Assembly.GetName().Name + ".Resources.Resources",
        typeof(LocalizationService).Assembly);

    public LocalizationService(IOptions<LocalizationOptions> options)
    {
        _globalEnglishTerms = new HashSet<string>(
            options.Value.GlobalEnglishTerms ?? new HashSet<string>(),
            System.StringComparer.OrdinalIgnoreCase);
    }

    private List<string> GetTextsKeys()
    {
        var resourceSet = ResourceManager.GetResourceSet(CultureInfo.CurrentCulture, true, true);
        List<string> keys = new List<string>();

        if (resourceSet != null)
        {
            var enumerator = resourceSet.GetEnumerator();

            if (enumerator != null)
            {
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current != null && enumerator.Current is DictionaryEntry entry)
                    {
                        var key = entry.Key?.ToString();
                        if (!string.IsNullOrEmpty(key))
                        {
                            keys.Add(key);
                        }
                    }
                }
            }
        }

        return keys;
    }


    public IDictionary<string, string> GetTexts(string languageCode)
    {
        var culture = GetCultureInfo(languageCode);
        var result = new Dictionary<string, string>(System.StringComparer.Ordinal);

        foreach (var key in GetTextsKeys())
        {
            var value = ResourceManager.GetString(key, culture);
            if (value != null)
            {
                result[key] = value;
            }
            else
            {
                // Fallback to English if key not found
                result[key] = ResourceManager.GetString(key, CultureInfo.GetCultureInfo("en")) ?? key;
            }
        }

        return result;
    }

    public IDictionary<string, string> TranslateTexts(string languageCode, IEnumerable<string> sourceTexts)
    {
        var culture = GetCultureInfo(languageCode);
        var result = new Dictionary<string, string>(System.StringComparer.Ordinal);

        foreach (var text in sourceTexts ?? Array.Empty<string>())
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                continue;
            }

            var trimmedText = text.Trim();

            // Check if this term should always remain in English
            if (_globalEnglishTerms.Contains(trimmedText))
            {
                result[text] = text;
                continue;
            }

            var found = false;
            foreach (var key in GetTextsKeys())
            {
                var englishValue = ResourceManager.GetString(key, CultureInfo.GetCultureInfo("en"));
                if (englishValue != null && englishValue.Equals(trimmedText, System.StringComparison.Ordinal))
                {
                    var translated = ResourceManager.GetString(key, culture);
                    result[text] = translated ?? englishValue;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                result[text] = text;
            }
        }

        return result;
    }

    private static CultureInfo GetCultureInfo(string? languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            return CultureInfo.GetCultureInfo("en");
        }

        var lang = languageCode.Trim().ToLowerInvariant();
        try
        {
            return CultureInfo.GetCultureInfo(lang);
        }
        catch (CultureNotFoundException)
        {
            return CultureInfo.GetCultureInfo("en");
        }
    }
}


