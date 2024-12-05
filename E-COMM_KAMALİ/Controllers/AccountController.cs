using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ECOMM.Core.Models;
using System.Threading.Tasks;
using ECOMM.Business.Concrete;
using ECOMM.Business.Abstract;
using Microsoft.EntityFrameworkCore;

namespace ECOMM.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        public async Task<IActionResult> SendVerificationCode(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View();
            }

            var code = GenerateVerificationCode();
            var subject = "Your Verification Code";
            var body = $"Your verification code is {code}";

            // Doğrulama kodunu veritabanına kaydedelim
            var emailVerification = new EmailVerification
            {
                UserId = user.Id,
                VerificationCode = code,
                CreatedAt = DateTime.Now,
                ExpirationTime = DateTime.Now.AddMinutes(15) // 15 dakika geçerlilik süresi
            };

            // Servis aracılığıyla veritabanına kaydedin
            await _emailService.AddVerificationCodeAsync(emailVerification);

            // E-posta gönderme işlemi
            await _emailService.SendEmailAsync(email, subject, body);

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(string verificationCode)
        {
            var emailVerification = await _emailService.GetVerificationCodeAsync(verificationCode);

            if (emailVerification != null)
            {
                // Başarılı doğrulama işlemi
                return RedirectToAction("Success");
            }
            else
            {
                // Kod hatalı veya süresi dolmuş
                ModelState.AddModelError("", "The verification code is incorrect or has expired.");
                return View();
            }
        }

        private string GenerateVerificationCode()
        {
            var random = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] byteArray = new byte[4];
            random.GetBytes(byteArray);
            int number = BitConverter.ToInt32(byteArray, 0) & 0x7FFFFFFF;  // Pozitif sayı üretmek için
            return (number % 1000000).ToString("D6"); // 6 haneli kod üretme
        }


















        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null && !user.EmailConfirmed)  // Email doğrulama kontrolü
                    {
                        // Kullanıcı doğrulama kodu bekliyor
                        return RedirectToAction("SendVerificationCode", new { email = model.Email });
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Mail veya şifre yanlış");
                    return View(model);
                }
            }
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,  
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }

        public IActionResult VerifyEmail()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
          
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Bir şeyler ters gitti");
                    return View(model);
                }
                else
                {
                  
                    return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
                }
           
          
                ModelState.AddModelError("", "Maildesorunvarbro");
          
            return View(model);
        }

     

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyEmail", "Account");
            }
            return View(new ChangePasswordViewModel { Email = username });
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user != null)
                {
                    var result = await _userManager.RemovePasswordAsync(user);
                    if (result.Succeeded)
                    {
                        result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (var error in result.Errors) { ModelState.AddModelError("", error.Description); }
                    }
                    return View(model);
                }
                else
                {
                    { ModelState.AddModelError("", "Email bulunamadı."); }
                    return View(model);
                }
            }
            else
            {
                { ModelState.AddModelError("", "Bir şeyler ters gitti. Tekrar Deneyin."); }
                return View(model);
            }

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

       
    }
}
