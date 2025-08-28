using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMH_ERP.Domain.Common;

namespace KMH_ERP.Domain.Entities
{
    public class ActivityLog
    {
        public int ActivityLogId { get; set; }
        public int UserId { get; set; }
        public string Action { get; set; } = null!;
        public string? Controller { get; set; }
        public DateTime ActionDate { get; set; }
        public string? IPAddress { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public User User { get; set; } = null!;
    }
}
