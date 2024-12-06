using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.Models
{
    public class EmailVerification
    {
        public int Id { get; set; }
        public string UserId { get; set; } // Kullanıcıya ait ID
        public string VerificationCode { get; set; }
        public DateTime CreatedAt { get; set; } // Kodu oluşturma zamanı
        public DateTime ExpirationTime { get; set; } // Kodun geçerlilik süresi
        public bool IsUsed { get; set; }
    }

}
