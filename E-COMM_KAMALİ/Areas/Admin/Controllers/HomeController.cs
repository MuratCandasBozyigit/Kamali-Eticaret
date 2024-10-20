using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace ECOMM.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]")]
    
    public class HomeController : AdminBaseController
    {
        public HomeController(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
