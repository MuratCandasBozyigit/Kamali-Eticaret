using Microsoft.AspNetCore.Mvc;
using ECOMM.Business.Abstract;
using ECOMM.Core.Models;

namespace E_COMM_KAMALİ.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public OrderController(IOrderService orderService, ICategoryService categoryService, IProductService productService, IUserService userService)
        {
            _orderService = orderService;
            _categoryService = categoryService;
            _productService = productService;
            _userService = userService;
        }

        // Tüm siparişleri listeleyen sayfa
        public async Task<IActionResult> Index()
        {
            var allOrders = await _orderService.GetAllAsync();
            return View(allOrders);
        }

        // Belirli bir siparişi detaylı görüntüleme sayfası
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        // Yeni sipariş ekleme sayfası
        public async Task<IActionResult> AddAsync(Orders orders)
        {
            if (orders == null)
            {
                return BadRequest("Orders cannot be null.");
            }

            var addedOrder = await _orderService.AddAsync(orders);
            return RedirectToAction("Index"); // Başarılı eklemeden sonra Index sayfasına yönlendirme yapıyoruz
        }

        // Sipariş silme işlemi
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var isDeleted = await _orderService.DeleteAsync(id);
            if (!isDeleted)
            {
                return BadRequest("Sipariş silinemedi.");
            }
            return RedirectToAction("Index"); // Başarılı silme işleminden sonra Index sayfasına yönlendirme yapıyoruz
        }

        // Sipariş düzenleme sayfası
        public async Task<IActionResult> EditAsync(Orders orders)
        {
            var updatedOrder = await _orderService.UpdateAsync(orders);
            if (updatedOrder == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index"); // Başarılı güncellemeden sonra Index sayfasına yönlendirme yapıyoruz
        }
    }
}
