using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ECOMM.Core.Models;
using System.Threading.Tasks;
using ECOMM.Business.Abstract;
using System;
using ECOMM.Business.Concrete;
using ECOMM.Data.Migrations;

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
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // 6 haneli kod
        }

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
            // Kullanıcıya bir başarı mesajı göndermek için TempData'yı kullanıyoruz
            TempData["SuccessMessage"] = "Kod gönderildi! Lütfen e-posta kutunuzu kontrol edin.";
            TempData["Email"] = email;
            return RedirectToAction("VerifyCode");
        }

        [HttpGet]
        public IActionResult VerifyCode()
        {
            return View(new VerifyCodeViewModel { Email = TempData["Email"]?.ToString() });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(string verificationCode)
        {
            if (TempData["UserId"] == null)
            {
                return RedirectToAction("Login"); // Giriş yapılmadan doğrulama sayfasına gelinemez
            }

            var userId = (string)TempData["UserId"];
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ModelState.AddModelError("", "Geçersiz kullanıcı.");
                return RedirectToAction("Login");
            }

            // Veritabanında doğrulama kodunu ara
            var emailVerification = await _emailService.GetVerificationCodeAsync(user.Id);

            if (emailVerification == null || emailVerification.IsUsed || emailVerification.VerificationCode != verificationCode)
            {
                ModelState.AddModelError("", "Geçersiz veya kullanılmış doğrulama kodu.");
                return View(); // Kod yanlışsa, tekrar giriş yapılabilir
            }

            if (emailVerification.ExpirationTime < DateTime.Now)
            {
                ModelState.AddModelError("", "Doğrulama kodu süresi dolmuş.");
                return View(); // Kodun süresi dolmuşsa tekrar giriş yapılabilir
            }

            // Kod doğrulandıysa, kullanıcıyı giriş yapmış say
            await _signInManager.SignInAsync(user, isPersistent: false);

            // Kodun kullanıldığını işaretle
            emailVerification.IsUsed = true;
            await _emailService.UpdateVerificationCodeAsync(emailVerification);

            // Anasayfaya yönlendir
            return RedirectToAction("Index", "Home");
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

                // Şifreyi kontrol et, ancak oturum açma işlemi yapma
                var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                if (result.Succeeded)
                {
                    // Her girişte yeni doğrulama kodu gönder
                    var code = GenerateVerificationCode();
                    var emailVerification = new EmailVerification
                    {
                        UserId = user.Id,
                        VerificationCode = code,
                        CreatedAt = DateTime.Now,
                        ExpirationTime = DateTime.Now.AddMinutes(15),
                        IsUsed = false
                    };

                    // Veritabanına doğrulama kodunu ekle
                    await _emailService.AddVerificationCodeAsync(emailVerification);
                    // Kullanıcıya doğrulama kodunu gönder
                    await _emailService.SendVerificationCodeAsync(user.Email, code);

                    // Kullanıcı bilgilerini geçici veri olarak tut
                    TempData["UserId"] = user.Id;

                    // Doğrulama sayfasına yönlendir
                    return RedirectToAction("VerifyCode");
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
                    UserName = model.Email
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    TempData["Email"] = model.Email;
                    return RedirectToAction("SendVerificationCode", new { email = model.Email });
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }

        public IActionResult ChangePassword(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("VerifyCode");
            }

            return View(new ChangePasswordViewModel { Email = username });
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return View(model);
                }

                var removeResult = await _userManager.RemovePasswordAsync(user);
                if (removeResult.Succeeded)
                {
                    var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                    if (addResult.Succeeded)
                    {
                        TempData["Message"] = "Şifre başarıyla değiştirildi.";
                        return RedirectToAction("Login");
                    }
                }

                ModelState.AddModelError("", "Şifre değiştirilemedi.");
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}
