using KMH_ERP.Application.DTOs;

namespace KMH_ERP.Web.ViewModels
{
    public class RoleViewModel : BaseViewModel
    {
        public RoleDto Role { get; set; } = new RoleDto();

        public List<RoleDto> RoleList { get; set; } = new List<RoleDto>();
    }
}
