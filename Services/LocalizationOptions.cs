namespace YelhighWebsite.Services;

/// <summary>
/// Configuration options for localization service.
/// </summary>
public class LocalizationOptions
{
    /// <summary>
    /// Terms that should always remain in English regardless of the selected language.
    /// These are typically widely recognized brand names, technologies, or terms.
    /// </summary>
    public HashSet<string> GlobalEnglishTerms { get; set; } = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase);
}

