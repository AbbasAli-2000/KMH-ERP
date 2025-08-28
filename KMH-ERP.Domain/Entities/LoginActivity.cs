using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMH_ERP.Domain.Common;

namespace KMH_ERP.Domain.Entities
{
    public class LoginActivity
    {
        public int LoginActivityId { get; set; }
        public int? UserId { get; set; }
        public string? Username { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string? IPAddress { get; set; }
        public string? DeviceName { get; set; } = Environment.MachineName;
        public bool IsSuccess { get; set; }
        public string? FailureReason { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.Now;
        public User? User { get; set; }
    }
}
