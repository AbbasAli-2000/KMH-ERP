using KMH_ERP.Infrastructure.Utilities.Halpers;
using KMH_ERP.Infrastructure.Utilities.Helpers;

namespace KMH_ERP.Web.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //var path = context.Request.Path.Value?.ToLower() ?? "";

            //// Skip public pages, login/logout, static files
            //var publicPaths = new[] { "/", "/account/login", "/account/logout" };
            //var staticFolders = new[] { "/css", "/js", "/images", "/lib" };

            //if (publicPaths.Contains(path) || staticFolders.Any(f => path.StartsWith(f)))
            //{
            //    await _next(context);
            //    return;
            //}

            //// Get session and token
            //var userId = SessionHelper.GetString("UserID");
            //var token = CookieHelper.GetCookie("Token");

            //if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            //{
            //    // User not logged in → redirect to login
            //    context.Response.Redirect("/Account/Login");
            //    return;
            //}

            //bool tokenValid = JwtHelper.ValidateToken(token);
            //if (!tokenValid)
            //{
            //    // Token expired or invalid → clear session + cookies, redirect
            //    SessionHelper.Clear();
            //    CookieHelper.RemoveCookie("Token");
            //    context.Response.Redirect("/Account/Login");
            //    return;
            //}

            // User is logged in & token valid → continue
            await _next(context);
        }
    }
}
