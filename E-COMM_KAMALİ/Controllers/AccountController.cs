using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ECOMM.Core.Models;
using System.Threading.Tasks;
using ECOMM.Business.Abstract;
using System;
using ECOMM.Business.Concrete;

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

            // E-posta ve başarı mesajını Session'a kaydediyoruz
            HttpContext.Session.SetString("Email", email);
            HttpContext.Session.SetString("SuccessMessage", "Kod gönderildi! Lütfen e-posta kutunuzu kontrol edin.");

            return RedirectToAction("VerifyCode");
        }








        #region VerifyCode
        [HttpGet]
        public IActionResult VerifyCode()
        {
            var email = TempData["Email"] as string;  // TempData'dan e-posta bilgisini alıyoruz
            if (email == null)
            {
                return RedirectToAction("Login");  // Eğer email yoksa, Login sayfasına yönlendiriyoruz
            }

            // Başarı mesajını ViewBag ile gönderiyoruz
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            TempData.Keep("SuccessMessage");  // SuccessMessage'ı koruyoruz

            return View(new VerifyCodeViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)  // Model geçerli değilse, hata mesajlarını göster
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View(model);
            }

            var verificationRecord = await _emailService.GetVerificationCodeAsync(user.Id, model.VerificationCode);
            if (verificationRecord == null || verificationRecord.IsUsed || verificationRecord.ExpirationTime < DateTime.Now)
            {
                ModelState.AddModelError("", "Geçersiz veya süresi dolmuş doğrulama kodu.");
                return View(model);
            }

            verificationRecord.IsUsed = true;
            await _emailService.UpdateVerificationCodeAsync(verificationRecord);

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        #endregion

        #endregion

        #region Account
        #region Login

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

                var passwordCheck = await _userManager.CheckPasswordAsync(user, model.Password);
                if (!passwordCheck)
                {
                    ModelState.AddModelError("", "E-posta veya şifre yanlış.");
                    return View(model);
                }

                // Kullanıcı doğruysa doğrulama kodu gönder
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
                await _emailService.SendVerificationCodeAsync(user.Email, code);

                // Kullanıcıyı doğrulama kodu sayfasına yönlendir
                TempData["Email"] = user.Email;  // Burada TempData kullanarak e-posta bilgisini taşıyoruz
                return RedirectToAction("VerifyCode");  // Yönlendirme işlemi
            }

            return View(model);
        }

        #endregion


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
                    TempData["Email"] = model.Email;  // E-posta bilgisini TempData'ya taşıyoruz

                    // Burada SendVerificationCode metodunu doğrudan çağırıyoruz
                    return await SendVerificationCode(model.Email);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
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
