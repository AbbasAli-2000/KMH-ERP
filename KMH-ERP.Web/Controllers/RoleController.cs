using KMH_ERP.Application.Interfaces.Services;
using KMH_ERP.Infrastructure.Utilities.Helpers;
using KMH_ERP.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KMH_ERP.Web.Controllers
{
    public class RoleController : Controller
    {

        private readonly IRoleService _roleService;
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new RoleViewModel
            {
                RoleList = await _roleService.GetRolesAsync(),
                SuccessMessage = TempData["SuccessMessage"] as string,
                ErrorMessage = TempData["ErrorMessage"] as string
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> AddRole(string? id)
        {
            var roleVM = new RoleViewModel();
            if (id != null)
            {
                int roleId = Convert.ToInt32(EncryptionHelper.Decrypt(id));
                var role = await _roleService.FindRoleAsync(roleId);
                if (role != null)
                {
                    roleVM.Role = role;
                }
            }
            return View(roleVM);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddRole(RoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = SessionHelper.GetString("UserID");
            model.Role.UserId = Convert.ToInt32(userId);
            var result = await _roleService.SaveRoleAsync(model.Role);
            if (result > 0)
            {
                TempData["SuccessMessage"] = model.Role.RoleId == 0 ? "Role saved successfully." : "Role updated successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                model.ErrorMessage = "Failed to save role.";
                return View(model);
            }
        }
    }
}
