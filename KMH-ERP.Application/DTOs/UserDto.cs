#nullable disable

using System.ComponentModel.DataAnnotations;

namespace KMH_ERP.Application.DTOs
{
    public class UserDto
    {
        public int UserId { get; set; }
        public int? EmployeeId { get; set; }

        public string UserName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;
        public string Email { get; set; } = null!;

        public string PhoneNumber { get; set; }

        public string MobileNumber { get; set; }
        public int? RoleId { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool IsRemote { get; set; }
        public DateTime? LockDate { get; set; }
        public DateTime? LastLogin { get; set; }

        public int FailedLoginAttempts { get; set; }
        public string RoleName { get; set; }
        public string ProfileImagePath { get; set; }

    }
}
