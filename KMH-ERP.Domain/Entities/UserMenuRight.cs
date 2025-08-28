using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMH_ERP.Domain.Common;

namespace KMH_ERP.Domain.Entities
{
    public class UserMenuRight : BaseEntity
    {
        public int MenuRightId { get; set; } 
        public int UserId { get; set; }
        public int MenuId { get; set; }
        public bool CanView { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        public User User { get; set; } = null!;
        public Menu Menu { get; set; } = null!;

    }
}
