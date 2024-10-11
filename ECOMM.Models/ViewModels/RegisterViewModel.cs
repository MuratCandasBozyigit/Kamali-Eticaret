using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "isim girmeniz gerekli")]
        public string Name { get; set; }
        //[Required(ErrorMessage = "Telefon numarası girmeniz gerekli")]
       // public int PhoneNumber { get; set; }

        [Required(ErrorMessage = "Mail girmeniz gerekli")]
        [EmailAddress(ErrorMessage = "Geçersiz email formatı")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre girmeniz gerekli")]
        [StringLength(40,MinimumLength =8,ErrorMessage ="The {0} must be at {2} and at max {1} character")]
        [DataType(DataType.Password)]
        [Compare("ConfirmPassword",ErrorMessage ="Şifreler eşleşmiyor")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Şifreyi tekrar girmeniz gerekli")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni şifreyi onaylayın.")]
        public string ConfirmPassword { get; set; }
    }
}
