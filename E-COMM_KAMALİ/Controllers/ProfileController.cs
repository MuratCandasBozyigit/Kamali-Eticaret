using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ECOMM.Core.Models; // Model klasöründeki User modelini dahil et
using ECOMM.Core.ViewModels;
using ECOMM.Business.Abstract; // ViewModel klasöründeki ViewModel'i dahil et

public class ProfileController : Controller
{
    private readonly ICommentService _commentService;
    private readonly UserManager<User> _userManager; // ASP.NET Identity UserManager
  //  private readonly SignInManager<User> _signInManager; // SignInManager
    public ProfileController(UserManager<User> userManager, /*SignInManager<User> signInManager,*/ ICommentService commentService)
    {
        _userManager = userManager;
      //  _signInManager = signInManager;
        _commentService = commentService;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return RedirectToAction("Login", "Account");
        }

        var userId = user.Id;
        var comments = await _commentService.GetUserCommentsAsync(userId);

        // Profil bilgilerini ve yorumları ViewModel'e dahil et
        var viewModel = new ProfileViewModel
        {
            Email = user.Email,
            FullName = user.FullName,
            UserOrders = user.UserOrders,
            Comments = comments // Yorumları ViewModel'e ekliyoruz
        };

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
              
                user.Email = model.Email;
                user.FullName = model.FullName;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Json(new
                    {
                        success = true,
                      
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

    public async Task<IActionResult> GetComments()
    {
        var user = await _userManager.GetUserAsync(User); // Giriş yapan kullanıcıyı al
        if (user == null)
        {
            return Json(new { data = new List<object>() }); // Kullanıcı yoksa boş liste döndür
        }

      
        var comments = await _commentService.GetUserCommentsAsync(user.Id); // UserId'ye göre yorumları al
        var result = comments.Select(c => new
        {
            c.Content,
            ProductName = c.Product?.ProductName ?? "Bilinmiyor",
            c.DateCommented,
            c.IsApproved
        });

        return Json(new { data = result });
    }



}

