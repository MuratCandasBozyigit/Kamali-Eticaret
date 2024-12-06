using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ECOMM.Core.Models;
using System.Threading.Tasks;
using ECOMM.Business.Abstract;
using Microsoft.EntityFrameworkCore;
using System;

namespace ECOMM.Web.Controllers
{
    public class AccountController : Controller
    {
        #region Services
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }
        #endregion

        #region Email Verification

        private string GenerateVerificationCode()
        {
            var random = new System.Security.Cryptography.RNGCryptoServiceProvider();
            byte[] byteArray = new byte[4];
            random.GetBytes(byteArray);
            int number = BitConverter.ToInt32(byteArray, 0) & 0x7FFFFFFF;
            return (number % 1000000).ToString("D6"); // 6 haneli doğrulama kodu
        }

        // Doğrulama kodu gönderme
        [HttpPost]
        public async Task<IActionResult> SendVerificationCode(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError("", "E-posta adresi geçersiz.");
                return View();
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View();
            }

            var code = GenerateVerificationCode();
            var emailVerification = new EmailVerification
            {
                UserId = user.Id,
                VerificationCode = code,
                CreatedAt = DateTime.Now,
                ExpirationTime = DateTime.Now.AddMinutes(15),
                IsUsed = false
            };

            await _emailService.AddVerificationCodeAsync(emailVerification);
            await _emailService.SendVerificationCodeAsync(email, code);

            return RedirectToAction("VerifyCode", new { email });
        }
        // Kod doğrulama
        [HttpPost]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emailVerification = await _emailService.GetVerificationCodeAsync(model.VerificationCode);
                if (emailVerification != null)
                {
                    await _emailService.MarkCodeAsUsedAsync(model.VerificationCode);

                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null && !user.EmailConfirmed)
                    {
                        user.EmailConfirmed = true;
                        await _userManager.UpdateAsync(user);
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz veya süresi dolmuş doğrulama kodu.");
                }
            }

            return View(model);
        }
        // Email ile giriş yapılacak sayfa
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var code = GenerateVerificationCode();
                    var emailVerification = new EmailVerification
                    {
                        UserId = user.Id,
                        VerificationCode = code,
                        CreatedAt = DateTime.Now,
                        ExpirationTime = DateTime.Now.AddMinutes(15),
                        IsUsed = false
                    };

                    await _emailService.AddVerificationCodeAsync(emailVerification);
                    await _emailService.SendVerificationCodeAsync(model.Email, code);

                    return RedirectToAction("VerifyCode", new { email = model.Email });
                }
                else
                {
                    ModelState.AddModelError("", "E-posta adresi bulunamadı.");
                }
            }
            return View(model);
        }
        #endregion

        #region Account

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    ModelState.AddModelError("", "E-posta adresi bulunamadı.");
                    return View(model);
                }

                // Kullanıcının e-posta doğrulaması yapılmamışsa giriş yapılmasın
                if (!user.EmailConfirmed)
                {
                    ModelState.AddModelError("", "Hesabınız henüz doğrulanmadı. Lütfen e-posta adresinizi doğrulayın.");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "E-posta veya şifre yanlış.");
            }
            return View(model);
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FullName = model.Name,
                    Email = model.Email,
                    UserName = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

       
  

        // Şifre değiştirme
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
                var user = await _userManager.FindByEmailAsync(model.Email);
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
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("", "Email bulunamadı.");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Bir şeyler ters gitti. Tekrar deneyin.");
            return View(model);
        }

      

        #endregion
    }
}
