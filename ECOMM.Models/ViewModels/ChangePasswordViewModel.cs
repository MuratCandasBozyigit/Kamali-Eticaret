using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Mail girmeniz gerekli")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre girmeniz gerekli")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "The {0} must be at {2} and at max {1} character")]
        [DataType(DataType.Password)]
        [Compare("ConfirmNewPassword", ErrorMessage = "Şifreler eşleşmiyor")]
        [Display(Name = "Yeni şifre.")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Şifreyi tekrar girmeniz gerekli")]
        [DataType(DataType.Password)]
        [Display(Name ="Yeni şifreyi onaylayın.")]
        public string ConfirmNewPassword { get; set; }
    }
}
