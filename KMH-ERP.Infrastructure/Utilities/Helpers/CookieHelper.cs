using Microsoft.AspNetCore.Http;

namespace KMH_ERP.Infrastructure.Utilities.Helpers
{
    public static class CookieHelper
    {
        private static IHttpContextAccessor? _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        //Set a cookies
        public static void SetCookies(string key, string value, int? expireMinutes = null)
        {
            if (_httpContextAccessor.HttpContext == null) { return; }
            var options = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                Expires = expireMinutes.HasValue ? DateTimeOffset.Now.AddMinutes(expireMinutes.Value) : (DateTimeOffset?)null
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, options);
        }

        //Get a Cookies
        public static string? GetCookie(string key)
        {
            if (_httpContextAccessor.HttpContext == null) return null;

            return _httpContextAccessor.HttpContext.Request.Cookies[key];
        }

        //Delete a cookie
        public static void RemoveCookie(string key)
        {
            if (_httpContextAccessor.HttpContext == null) return;

            _httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

    }
}
