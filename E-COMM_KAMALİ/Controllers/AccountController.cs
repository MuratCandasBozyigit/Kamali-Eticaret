using ECOMM.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using ECOMM.Core.Models;
using ECOMM.Business.Concrete;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace ECOMM.Web.Controllers
{
    public class AccountController : Controller
    {
        #region BİTTİ
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

        #region SendVerification
        #region GenerateRandomNumber
        private string GenerateVerificationCode()
        {
            // RandomNumberGenerator ile 6 haneli bir sayı üretir.
            byte[] randomNumber = new byte[4]; // 4 byte, büyük bir sayı üretmek için yeterlidir.
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            // Byte dizisini bir tam sayıya dönüştürür ve 6 haneli bir sayı alır.
            int numericCode = Math.Abs(BitConverter.ToInt32(randomNumber, 0)) % 1000000;

            // Kodun tam 6 haneli olmasını sağlamak için sıfırlarla doldurur.
            return numericCode.ToString("D6");
        } 
        #endregion

        #region ŞifreGönder
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
        #endregion

        #region ŞifreDeğişmeKodGönderme
        [HttpPost]
        public async Task<IActionResult> SendVerificationCodeForChangePassword(string email)
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

            // Kod oluştur
            var code = GenerateVerificationCode();

            // Kod ve kullanıcı bilgileri ile verification kaydını oluşturuyoruz
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

            HttpContext.Session.SetString("Email", email);
            TempData["SuccessMessage"] = "Kod gönderildi! Lütfen e-posta kutunuzu kontrol edin.";

            return RedirectToAction("VerifyCodeForChangePassword"); // Kullanıcıyı doğrulama sayfasına yönlendir
        }

        #endregion

        #region ŞifreDeğişmeKodDoğrulama

        [HttpGet]
        public IActionResult VerifyCodeForChangePassword()
        {
            string email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                Console.WriteLine("Email is null or empty.");
                return RedirectToAction("Login"); // Redirect to login if email is missing
            }

            var model = new VerifyCodeViewModel { Email = email };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCodeForChangePassword(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
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

            // Kullanıcı giriş yaptırmadan, şifre değiştirme ekranına yönlendir
            TempData["Email"] = model.Email;
            return RedirectToAction("ChangePassword");
        }


        #endregion
        #endregion

        #region VerifyCode

        #region KodDoğrulama
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


        #region MailDoğrulama
        public IActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
        {
            if (ModelState.IsValid)
            {
                // E-posta adresine göre kullanıcıyı buluyoruz
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                    return View(model);
                }

                // Set the email to session
                HttpContext.Session.SetString("Email", model.Email);

                // Doğrulama kodu oluşturuyoruz
                var code = GenerateVerificationCode(); // Kendi kod oluşturma metodunuzu kullanın

                // Doğrulama kodunu ve diğer bilgileri kaydediyoruz
                var emailVerification = new EmailVerification
                {
                    UserId = user.Id,
                    VerificationCode = code,
                    CreatedAt = DateTime.Now,
                    ExpirationTime = DateTime.Now.AddMinutes(15), // 15 dakika geçerlilik süresi
                    IsUsed = false
                };

                // EmailVerification nesnesini veritabanına ekliyoruz
                await _emailService.AddVerificationCodeAsync(emailVerification);

                // Kullanıcıya doğrulama kodunu e-posta ile gönderiyoruz
                await _emailService.SendVerificationCodeAsync(user.Email, code);

                // Başarı mesajı ve yönlendirme
                TempData["Message"] = "Doğrulama kodu e-posta adresinize gönderildi.";
                return RedirectToAction("VerifyCodeForChangePassword", new { email = model.Email });
            }

            // Eğer model geçerli değilse formu yeniden göster
            return View(model);
        }


        #endregion
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

        #region Register
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
        #endregion

        #region ChangPwd

        [HttpGet]
        public IActionResult ChangePassword()
        {
            var email = TempData["Email"] as string;

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");
            }

            return View(new ChangePasswordViewModel { Email = email });
        }



        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Kullanıcı bulunamadı.");
                return View(model);
            }

            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded)
            {
                ModelState.AddModelError("", "Eski şifre kaldırılırken bir hata oluştu.");
                return View(model);
            }

            var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addResult.Succeeded)
            {
                ModelState.AddModelError("", "Yeni şifre belirlenirken bir hata oluştu.");
                return View(model);
            }

            TempData["Message"] = "Şifre başarıyla değiştirildi.";
            return RedirectToAction("Login");
        }




        #endregion

        #region Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        #endregion
        #endregion
        #endregion
    }
}
