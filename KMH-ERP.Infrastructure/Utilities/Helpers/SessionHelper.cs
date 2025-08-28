using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace KMH_ERP.Infrastructure.Utilities.Helpers
{
    public static class SessionHelper
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        // Configure in Program.cs
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private static ISession? Session => _httpContextAccessor?.HttpContext?.Session;

        // Set simple string
        public static void SetString(string key, string value)
        {
            Session?.SetString(key, value);
        }

        // Get simple string
        public static string? GetString(string key)
        {
            return Session?.GetString(key);
        }

        // Set object (serialize)
        public static void SetObject<T>(string key, T value)
        {
            var json = JsonSerializer.Serialize(value);
            Session?.SetString(key, json);
        }

        // Get object (deserialize)
        public static T? GetObject<T>(string key)
        {
            var json = Session?.GetString(key);
            return json == null ? default : JsonSerializer.Deserialize<T>(json);
        }

        // Remove specific key
        public static void Remove(string key)
        {
            Session?.Remove(key);
        }

        // Clear all session
        public static void Clear()
        {
            Session?.Clear();
        }
    }
}
