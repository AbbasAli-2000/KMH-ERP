#nullable disable

namespace KMH_ERP.Application.DTOs
{
    public class LoginActivityDto
    {
        public int? UserId { get; set; }
        public string Username { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogOutTime { get; set; }
        public string IPAddress { get; set; }
        public string DeviceName { get; set; }
        public bool IsSuccess { get; set; }
        public string FailureReason { get; set; }
    }
}
