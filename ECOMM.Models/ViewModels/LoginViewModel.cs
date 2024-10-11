using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email girmeniz gerekli")]
        [EmailAddress]
        public string  Email { get; set; }
        [Required(ErrorMessage = "Şifre girmeniz gerekli")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
