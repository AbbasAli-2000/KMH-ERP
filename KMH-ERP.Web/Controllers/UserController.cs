using KMH_ERP.Application.DTOs;
using KMH_ERP.Application.Interfaces.Services;
using KMH_ERP.Domain.Entities;
using KMH_ERP.Infrastructure.Utilities.Helpers;
using KMH_ERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace KMH_ERP.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public UserController(IUserService userService, IRoleService role)
        {
            _userService = userService;
            _roleService = role;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new UserViewModel
            {
                UserList = await _userService.GetUserAsync(),
                SuccessMessage = TempData["SuccessMessage"] as string,
                ErrorMessage = TempData["ErrorMessage"] as string
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AddEditUser(string? id)
        {

            var roleList = await _roleService.GetRolesAsync();
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            var userVM = new UserViewModel();
            if (id != null)
            {
                int userId = Convert.ToInt32(EncryptionHelper.Decrypt(id));
                var user = await _userService.FindUserAsync(userId);
                if (user != null)
                {
                    userVM.User = user;
                }
            }
            return View(userVM);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEditUser(UserViewModel model)
        {
            var roleList = await _roleService.GetRolesAsync();
            ViewBag.RoleList = new SelectList(roleList, "RoleId", "RoleName");
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool isExistEmail = await _userService.GetByEmailAsync(model.User.Email, model.User.UserId);
            if (isExistEmail)
            {
                model.ErrorMessage = "This email is already exits";
                return View(model);
            }
            bool isExistPNumber = await _userService.GetByPhoneNumberAsync(model.User.PhoneNumber, model.User.UserId);
            if (isExistPNumber)
            {
                model.ErrorMessage = "This phone number is already exits";
                return View(model);
            }


            var userId = SessionHelper.GetString("UserID");
            model.User.UserId = Convert.ToInt32(userId);
            var result = await _userService.SaveUserAsync(model.User);
            if (result > 0)
            {
                TempData["SuccessMessage"] = model.Role.RoleId == 0 ? "User register successfully." : "User updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                model.ErrorMessage = "Failed to save User.";
                return View(model);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            //var user = _users.FirstOrDefault(u => u.UserId == id);
            //if (user != null) _users.Remove(user);

            TempData["SuccessMessage"] = "User deleted successfully!";
            return RedirectToAction("Index");
        }
    }
}
