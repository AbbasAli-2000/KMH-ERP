using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KMH_ERP.Domain.Common;

namespace KMH_ERP.Domain.Entities
{
    public class JwtLoginToken
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiryAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public User User { get; set; } = null!;
    }
}
