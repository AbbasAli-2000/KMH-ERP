using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMH_ERP.Domain.Common;

namespace KMH_ERP.Domain.Entities
{
    public class Menu : BaseEntity
    {
        public int MenuId { get; set; } 
        public int? ParentMenuId { get; set; }
        public string MenuName { get; set; } = null!;
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public string? AreaName { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; }

        public Menu? ParentMenu { get; set; }
        public ICollection<Menu> SubMenus { get; set; } = new List<Menu>();
        public ICollection<UserMenuRight> UserMenuRights { get; set; } = new List<UserMenuRight>();

    }
}
