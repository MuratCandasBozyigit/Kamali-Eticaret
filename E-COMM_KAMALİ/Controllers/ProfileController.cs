using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECOMM.Core.Models; // Model klasöründeki User modelini dahil et
using ECOMM.Core.ViewModels; // ViewModel klasöründeki ViewModel'i dahil et

public class ProfileController : Controller
{
    private readonly UserManager<User> _userManager; // ASP.NET Identity UserManager
    private readonly SignInManager<User> _signInManager; // SignInManager

    public ProfileController(UserManager<User> userManager, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // Profil sayfası
    public async Task<IActionResult> Index()
    {
        // Giriş yapmış kullanıcıyı al
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            // Kullanıcı bulunamadıysa login sayfasına yönlendir
            return RedirectToAction("Login", "Account");
        }
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account");
        }


        // User bilgilerini ViewModel'e dönüştür
        var viewModel = new ProfileViewModel
        {
           // UserName = user.UserName,
            Email = user.Email,
            FullName = user.FullName,
            UserOrders = user.UserOrders

            // Diğer alanları da buraya ekleyebilirsiniz
        };

        // Profil sayfasını view ile döndür
        return View(viewModel);
    }


 
    [HttpPost]
    public async Task<IActionResult> Edit(ProfileViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                user.UserName = model.UserName;
                user.Email = model.Email;
                user.FullName = model.FullName;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(new
                    {
                        success = true,
                        updatedUserName = user.UserName,
                        updatedEmail = user.Email,
                        updatedFullName = user.FullName
                    });
                }
                else
                {
                    return Json(new { success = false });
                }
            }
        }

        return Json(new { success = false });
    }



}

