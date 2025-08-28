using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KMH_ERP.Domain.Common;

namespace KMH_ERP.Domain.Entities
{
    public class User : BaseEntity
    {
        public int UserId { get; set; }

        public int? EmployeeId { get; set; }       
        public string UserName { get; set; } = null!;
        
        public string PasswordHash { get; set; } = null!;
        public string Email { get; set; } = null!;        
        public string? PhoneNumber { get; set; }

        public string? MobileNumber { get; set; }        
        public int? RoleId { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsLocked { get; set; } = false;
        public bool IsRemote { get; set; } = false;
        public DateTime? LockDate { get; set; }
        public int FailedLoginAttempts { get; set; }
        public DateTime? LastLogin { get; set; }

        // Navigation properties
        public Role Role { get; set; } = null!;
        public ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();
        public ICollection<LoginActivity> LoginActivities { get; set; } = new List<LoginActivity>();
        public ICollection<UserMenuRight> UserMenuRights { get; set; } = new List<UserMenuRight>();
    }
}
