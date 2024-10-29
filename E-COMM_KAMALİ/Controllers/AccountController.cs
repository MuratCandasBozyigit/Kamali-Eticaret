//using ECOMM.Core.ViewModels;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Identity;
//using ECOMM.Core.Models;
//using System.Threading.Tasks;

//namespace ECOMM.Web.Controllers
//{
//    public class AccountController : Controller
//    {
//        private readonly UserManager<User> _userManager;
//        private readonly SignInManager<User> _signInManager;

//        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//        }

//        // GET: Login
//        public IActionResult Login() => View();

//        // POST: Login
//        [HttpPost]
//        public async Task<IActionResult> Login(LoginViewModel model)
//        {
//            if (!ModelState.IsValid) return View(model);

//            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
//            if (result.Succeeded)
//            {
//                return RedirectToAction("Index", "Home");
//            }

//            ModelState.AddModelError("", "Mail veya şifre yanlış");
//            return View(model);
//        }

//        // GET: Register
//        public IActionResult Register() => View();

//        // POST: Register
//        [HttpPost]
//        public async Task<IActionResult> Register(RegisterViewModel model)
//        {
//            if (!ModelState.IsValid) return View(model);

//            var user = new User
//            {
//              
//                Email = model.Email,
//                UserName = model.UserName
//            };

//            var result = await _userManager.CreateAsync(user, model.Password);
//            if (result.Succeeded)
//            {
//                await _signInManager.SignInAsync(user, isPersistent: false);
//                TempData["SuccessMessage"] = "Kayıt başarılı! Giriş yapıldı.";
//                return RedirectToAction("Index", "Home");
//            }

//            foreach (var error in result.Errors)
//            {
//                ModelState.AddModelError("", error.Description);
//            }
//            return View(model);
//        }

//        // GET: VerifyEmail
//        public IActionResult VerifyEmail() => View();

//        // POST: VerifyEmail
//        [HttpPost]
//        public async Task<IActionResult> VerifyEmail(VerifyEmailViewModel model)
//        {
//            var user = await _userManager.FindByEmailAsync(model.Email);
//            if (user == null)
//            {
//                ModelState.AddModelError("", "E-posta adresi bulunamadı.");
//                return View(model);
//            }

//            return RedirectToAction("ChangePassword", "Account", new { username = user.UserName });
//        }

//        // GET: ChangePassword
//        public IActionResult ChangePassword(string username)
//        {
//            if (string.IsNullOrEmpty(username))
//            {
//                return RedirectToAction("VerifyEmail", "Account");
//            }

//            return View(new ChangePasswordViewModel { Email = username });
//        }

//        // POST: ChangePassword
//        [HttpPost]
//        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
//        {
//            if (!ModelState.IsValid)
//            {
//                ModelState.AddModelError("", "Formu eksiksiz doldurun.");
//                return View(model);
//            }

//            var user = await _userManager.FindByNameAsync(model.Email);
//            if (user == null)
//            {
//                ModelState.AddModelError("", "E-posta bulunamadı.");
//                return View(model);
//            }

//            var removeResult = await _userManager.RemovePasswordAsync(user);
//            if (!removeResult.Succeeded)
//            {
//                foreach (var error in removeResult.Errors)
//                    ModelState.AddModelError("", error.Description);
//                return View(model);
//            }

//            var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
//            if (!addResult.Succeeded)
//            {
//                foreach (var error in addResult.Errors)
//                    ModelState.AddModelError("", error.Description);
//                return View(model);
//            }

//            return RedirectToAction("Login", "Account");
//        }

//        // GET: Logout
//        public async Task<IActionResult> Logout()
//        {
//            await _signInManager.SignOutAsync();
//            return RedirectToAction("Index", "Home");
//        }
//    }
//}
