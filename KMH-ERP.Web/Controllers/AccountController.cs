using KMH_ERP.Application.DTOs;
using KMH_ERP.Application.Interfaces.Services;
using KMH_ERP.Domain.Entities;
using KMH_ERP.Infrastructure.Utilities.Halpers;
using KMH_ERP.Infrastructure.Utilities.Helpers;
using KMH_ERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KMH_ERP.Web.Controllers
{
    public class AccountController : Controller
    {

        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Login()
        {
            var vm = new LoginViewModel();
            return View(vm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            var (user, token) = await _authService.LoginAsync(new UserLoginDto
            {
                Email = loginViewModel.Email,
                Password = loginViewModel.Password,
                ipAddress = ip,
            });
            if (user == null)
            {
                loginViewModel.ErrorMessage = "Invalid Email or Password!";
                return View(loginViewModel);
            }
            if (user.IsRemote == true)
            {
                loginViewModel.ErrorMessage = "Not allow remote login!";
                return View(loginViewModel);
            }
            else
            {
                SessionHelper.SetString("UserID", user.UserId.ToString());
                SessionHelper.SetString("UserName", user.UserName);
                SessionHelper.SetString("RoleName", user.RoleName);
                //SessionHelper.SetString("RoleID", user.role.ToString());
                //SessionHelper.SetString("token", token);
                CookieHelper.SetCookies("Token", token, 1440);
                var roleName = user.RoleName;
                if (roleName == "SuperAdmin")
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var userId = SessionHelper.GetString("UserID");
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (userId != null)
            {
                await _authService.LogoutAsync(Convert.ToInt32(userId.ToString()), ip);
            }
            HttpContext.Session.Clear();
            Response.Cookies.Delete("Token");
            return RedirectToAction("Login", "Account");
        }
    }
}
