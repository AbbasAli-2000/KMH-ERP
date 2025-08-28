#nullable disable

namespace KMH_ERP.Application.DTOs
{
    public class UserLoginDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string ipAddress { get; set; } = string.Empty;   
    }
}
