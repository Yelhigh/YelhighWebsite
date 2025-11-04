using System;
using YelhighWebsite.Services;

namespace YelhighWebsite.Services;

public sealed class NavigationService : INavigationService
{
    public string GetTargetUrl(string key)
    {
        if (string.IsNullOrWhiteSpace(key)) return "/";

        switch (key.Trim().ToLowerInvariant())
        {
            case "youtube":
                return "https://www.youtube.com/@yelhighmusic";
            case "github":
                return "https://github.com/Yelhigh";
            case "email":
                return "mailto:thenevesid@gmail.com";
            default:
                return "/";
        }
    }
}


