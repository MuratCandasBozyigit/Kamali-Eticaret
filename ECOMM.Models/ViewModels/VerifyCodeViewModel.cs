using System.ComponentModel.DataAnnotations;

namespace ECOMM.Core.ViewModels
{
    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "E-posta adresi gereklidir.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta adresi girin.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Doğrulama kodu gereklidir.")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Doğrulama kodu 6 haneli bir sayı olmalıdır.")]
        public string VerificationCode { get; set; }
    }
}