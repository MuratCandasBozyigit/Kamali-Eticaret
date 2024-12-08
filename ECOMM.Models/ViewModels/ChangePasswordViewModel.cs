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
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Şifre en az 8 karakter olmalı")]
        [DataType(DataType.Password)]
        [Compare("ConfirmNewPassword", ErrorMessage = "Şifreler eşleşmiyor")]
        [Display(Name = "Yeni şifre.")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Şifreyi tekrar girmeniz gerekli")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni şifreyi onaylayın.")]
        public string ConfirmNewPassword { get; set; }
    }

}
