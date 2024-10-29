using System.ComponentModel.DataAnnotations;

namespace ECOMM.Core.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "İsim girmeniz gerekli")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mail girmeniz gerekli")]
        [EmailAddress(ErrorMessage = "Geçersiz email formatı")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre girmeniz gerekli")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Şifre en az {2} ve en fazla {1} karakter olmalıdır.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Şifreyi tekrar girmeniz gerekli")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler eşleşmiyor")]
        [Display(Name = "Yeni şifreyi onaylayın.")]
        public string ConfirmPassword { get; set; }
    }
}
