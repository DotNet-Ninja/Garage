using System.Text.Json;

namespace Garage.Constants;

public static class Defaults
{
    public const string ApplicationName = "Garage";

    public static class Settings
    {
        public const int CacheTimeoutMinutes = 60;
        public const int PermanentCookieTimeoutDays = 365;
    }

    public static class Colors
    {
        public const string IconColor = "#bbbbbb";
    }

    public static class Sites
    {
        public static readonly Guid Id = Guid.Empty;
        public const string Name = "Default Site";
        public const string Slug = "default";
    }

    public static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true,
        DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        PropertyNameCaseInsensitive = true
    };
}