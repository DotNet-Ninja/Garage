using DotNetNinja.AutoBoundConfiguration;
using Garage.Constants;

namespace Garage.Configuration;

[AutoBind("Settings")]
public class Settings
{
    public int CacheTimeoutMinutes { get; set; } = Defaults.Settings.CacheTimeoutMinutes;
    public int PermanentCookieTimeoutDays { get; set; } = Defaults.Settings.PermanentCookieTimeoutDays;

    public TimeSpan CacheTimeout => TimeSpan.FromMinutes(CacheTimeoutMinutes);

    public string Version { get; set; } = "unknown";
}