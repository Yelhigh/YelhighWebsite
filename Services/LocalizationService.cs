using System.Collections.Generic;

namespace YelhighWebsite.Services;

public sealed class LocalizationService : ILocalizationService
{
    // Terms to always leave in English (widely recognized)
    private static readonly HashSet<string> GlobalEnglishTerms = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
    {
        "C# .NET",
        "DEVELOPER",
        "GitHub",
        "LinkedIn",
        "404",
        "Home",
        "React",
        "JavaScript",
        "TypeScript",
        "YouTube",
        "Spotify"
    };

    private static readonly IDictionary<string, IDictionary<string, string>> LanguageToTexts =
        new Dictionary<string, IDictionary<string, string>>(System.StringComparer.OrdinalIgnoreCase)
        {
            ["en"] = new Dictionary<string, string>
            {
                ["listen_tracks"] = "Listen to My Tracks",
                ["get_in_touch"] = "Get in Touch",
                ["view_projects"] = "View Projects",
                ["contact_me"] = "Contact Me"
            },
            ["de"] = new Dictionary<string, string>
            {
                ["listen_tracks"] = "Meine Tracks anhören",
                ["get_in_touch"] = "Kontakt aufnehmen",
                ["view_projects"] = "Projekte ansehen",
                ["contact_me"] = "Kontaktieren Sie mich"
            },
            ["pl"] = new Dictionary<string, string>
            {
                ["listen_tracks"] = "Posłuchaj moich utworów",
                ["get_in_touch"] = "Skontaktuj się",
                ["view_projects"] = "Zobacz projekty",
                ["contact_me"] = "Napisz do mnie"
            },
            ["fr"] = new Dictionary<string, string>
            {
                ["listen_tracks"] = "Écouter mes morceaux",
                ["get_in_touch"] = "Entrer en contact",
                ["view_projects"] = "Voir les projets",
                ["contact_me"] = "Contactez-moi"
            }
        };

    public IDictionary<string, string> GetTexts(string languageCode)
    {
        if (string.IsNullOrWhiteSpace(languageCode))
        {
            return LanguageToTexts["en"]; // default
        }

        if (LanguageToTexts.TryGetValue(languageCode.Trim().ToLowerInvariant(), out var texts))
        {
            return texts;
        }

        return LanguageToTexts["en"]; // fallback
    }

    public IDictionary<string, string> TranslateTexts(string languageCode, IEnumerable<string> sourceTexts)
    {
        var lang = (languageCode ?? "en").Trim().ToLowerInvariant();
        if (!LanguageToDynamic.TryGetValue(lang, out var dict))
        {
            dict = LanguageToDynamic["en"]; // fallback
        }

        var result = new Dictionary<string, string>(System.StringComparer.Ordinal);
        foreach (var text in sourceTexts ?? Array.Empty<string>())
        {
            if (string.IsNullOrWhiteSpace(text)) continue;
            if (GlobalEnglishTerms.Contains(text.Trim())) { result[text] = text; continue; }
            if (dict.TryGetValue(text, out var translated))
            {
                result[text] = translated;
            }
            else
            {
                // default passthrough preserves exact match when unknown
                result[text] = text;
            }
        }
        return result;
    }

    // Hand-authored phrase translations used for arbitrary text-node localization
    private static readonly IDictionary<string, IDictionary<string, string>> LanguageToDynamic =
        new Dictionary<string, IDictionary<string, string>>(System.StringComparer.OrdinalIgnoreCase)
        {
            ["en"] = new Dictionary<string, string>
            {
                ["Music Producer & Sound Designer"] = "Music Producer & Sound Designer",
                ["Creating sonic landscapes and crafting beats that move souls. From electronic waves to orchestral arrangements, I bring your audio vision to life. Specializing in EDM, Hip-Hop, and Cinematic Soundtracks."] = "Creating sonic landscapes and crafting beats that move souls. From electronic waves to orchestral arrangements, I bring your audio vision to life. Specializing in EDM, Hip-Hop, and Cinematic Soundtracks.",
                ["C# Software Engineer"] = "C# Software Engineer",
                ["Building robust, scalable applications with C# and .NET ecosystem. Expertise in ASP.NET Core, Entity Framework, and modern cloud architectures. Creating elegant solutions from backend APIs to responsive frontends."] = "Building robust, scalable applications with C# and .NET ecosystem. Expertise in ASP.NET Core, Entity Framework, and modern cloud architectures. Creating elegant solutions from backend APIs to responsive frontends",
                ["← Listen to me!"] = "← Listen to me!",
                ["← Check if it works for me :)"] = "← Check if it works for me :)",
                ["Oops! Page not found"] = "Oops! Page not found",
                ["Return to Home"] = "Return to Home"
            },
            ["de"] = new Dictionary<string, string>
            {
                ["Music Producer & Sound Designer"] = "Musikproduzent & Sounddesigner",
                ["Creating sonic landscapes and crafting beats that move souls. From electronic waves to orchestral arrangements, I bring your audio vision to life. Specializing in EDM, Hip-Hop, and Cinematic Soundtracks."] = "Ich kreiere Klanglandschaften und Beats, die die Seele berühren. Von elektronischen Wellen bis zu orchestralen Arrangements erwecke ich Ihre audiovisuellen Ideen zum Leben. Spezialisiert auf EDM, Hip-Hop und cinematische Soundtracks.",
                ["C# Software Engineer"] = "C# Softwareentwickler",
                ["Building robust, scalable applications with C# and .NET ecosystem. Expertise in ASP.NET Core, Entity Framework, and modern cloud architectures. Creating elegant solutions from backend APIs to responsive frontends."] = "Entwicklung robuster, skalierbarer Anwendungen mit C# und dem .NET-Ökosystem. Expertise in ASP.NET Core, Entity Framework und modernen Cloud-Architekturen. Elegante Lösungen von Backend-APIs bis zu responsiven Frontends.",
                ["← Listen to me!"] = "← Hör mir zu!",
                ["← Check if it works for me :)"] = "← Guck, ob das bei mir funktioniert :)",
                ["Oops! Page not found"] = "Ups! Seite nicht gefunden",
                ["Return to Home"] = "Zurück zur Startseite"
            },
            ["pl"] = new Dictionary<string, string>
            {
                ["Music Producer & Sound Designer"] = "Producent muzyczny i sound designer",
                ["Creating sonic landscapes and crafting beats that move souls. From electronic waves to orchestral arrangements, I bring your audio vision to life. Specializing in EDM, Hip-Hop, and Cinematic Soundtracks."] = "Tworzę dźwiękowe pejzaże i bity poruszające duszę. Od elektronicznych brzmień po orkiestracje — realizuję Twoją wizję dźwiękową. Specjalizuję się w EDM, hip-hopie i muzyce filmowej.",
                ["C# Software Engineer"] = "Programista C#",
                ["Building robust, scalable applications with C# and .NET ecosystem. Expertise in ASP.NET Core, Entity Framework, and modern cloud architectures. Creating elegant solutions from backend APIs to responsive frontends."] = "Buduję odporne i skalowalne aplikacje w ekosystemie C# i .NET. Doświadczenie w ASP.NET Core, Entity Framework oraz nowoczesnych architekturach chmurowych. Tworzę eleganckie rozwiązania od API po responsywne frontend-y.",
                ["← Listen to me!"] = "← Słuchaj mnie!",
                ["← Check if it works for me :)"] = "← Sprawdź, czy to u mnie działa :)",
                ["Oops! Page not found"] = "Ups! Strony nie znaleziono",
                ["Return to Home"] = "Wróć do strony głównej"
            },
            ["fr"] = new Dictionary<string, string>
            {
                ["Music Producer & Sound Designer"] = "Producteur de musique et designer sonore",
                ["Creating sonic landscapes and crafting beats that move souls. From electronic waves to orchestral arrangements, I bring your audio vision to life. Specializing in EDM, Hip-Hop, and Cinematic Soundtracks."] = "Je crée des paysages sonores et des rythmes qui touchent l'âme. Des vagues électroniques aux arrangements orchestraux, je donne vie à votre vision audio. Spécialisé en EDM, Hip-Hop et bandes originales cinématographiques.",
                ["C# Software Engineer"] = "Ingénieur logiciel C#",
                ["Building robust, scalable applications with C# and .NET ecosystem. Expertise in ASP.NET Core, Entity Framework, and modern cloud architectures. Creating elegant solutions from backend APIs to responsive frontends."] = "Création d'applications robustes et évolutives avec C# et l'écosystème .NET. Expertise en ASP.NET Core, Entity Framework et architectures cloud modernes. Des solutions élégantes, des API backend aux frontends réactifs.",
                ["← Listen to me!"] = "← Écoute-moi !",
                ["← Check if it works for me :)"] = "← Vérifie si ça marche pour moi :)",
                ["Oops! Page not found"] = "Oups ! Page introuvable",
                ["Return to Home"] = "Retour à l'accueil"
            }
        };
}


