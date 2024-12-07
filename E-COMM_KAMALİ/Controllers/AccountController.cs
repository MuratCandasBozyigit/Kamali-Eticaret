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
            var emailVerification = new Business.Concrete.EmailVerification
            {
                UserId = user.Id,
                VerificationCode = code,
                CreatedAt = DateTime.Now,
                ExpirationTime = DateTime.Now.AddMinutes(15),
                IsUsed = false
            };

            await _emailService.AddVerificationCodeAsync(emailVerification);
            await _emailService.SendVerificationCodeAsync(email, code);

            TempData["Email"] = email;
            return RedirectToAction("VerifyCode");
        }

        [HttpGet]
        public IActionResult VerifyCode()
        {
            return View(new VerifyCodeViewModel { Email = TempData["Email"]?.ToString() });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var emailVerification = await _emailService.GetVerificationCodeAsync(model.VerificationCode);
                if (emailVerification != null && emailVerification.ExpirationTime > DateTime.Now && !emailVerification.IsUsed)
                {
                    await _emailService.MarkCodeAsUsedAsync(model.VerificationCode);

                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        TempData["Email"] = model.Email;

                        // Kullanıcı doğrulandıktan sonra işlem yönlendirmesi
                        if (!user.EmailConfirmed)
                        {
                            user.EmailConfirmed = true;
                            await _userManager.UpdateAsync(user);
                        }

                        // Şifre değişikliği için yönlendirme kontrolü
                        if (TempData["ChangePassword"]?.ToString() == "true")
                        {
                            return RedirectToAction("ChangePassword", new { username = user.Email });
                        }

                        return RedirectToAction("Login");
                    }
                }

                ModelState.AddModelError("", "Geçersiz veya süresi dolmuş doğrulama kodu.");
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

                if (!user.EmailConfirmed)
                {
                    await SendVerificationCode(user.Email);
                    TempData["Message"] = "Lütfen e-posta doğrulama kodunu girin.";
                    return RedirectToAction("VerifyCode");
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
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
