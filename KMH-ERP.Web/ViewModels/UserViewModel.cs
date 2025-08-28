using KMH_ERP.Application.DTOs;

namespace KMH_ERP.Web.ViewModels
{
    public class UserViewModel : BaseViewModel
    {
        public RoleDto Role { get; set; } = new RoleDto();

        public List<RoleDto> RoleList { get; set; } = new List<RoleDto>();

        public UserDto User { get; set; } = new UserDto();

        public List<UserDto> UserList { get; set; } = new List<UserDto>();

        public string? confirmPassword { get; set; }

    }
}
