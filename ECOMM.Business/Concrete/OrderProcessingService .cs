using System.Threading.Tasks;
using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;

namespace ECOMM.Business.Concrete
{
    public class OrderProcessingService : IOrderProcessingService
    {
        private readonly IOrderService _orderService;

        public OrderProcessingService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<bool> ProcessOrderAsync(OrderSummaryViewModel orderSummary)
        {
            // Ödeme doğrulama ve sipariş kaydı işlemleri
            var isPaymentValid = await ValidatePaymentAsync(orderSummary);
            if (!isPaymentValid) return false;

            var newOrder = new Orders
            {
                UserId = orderSummary.ShippingAddress,
                OrderDate = System.DateTime.Now,
                TotalAmount = orderSummary.Total,
                Status = "Pending",
                ShippingAddress = orderSummary.ShippingAddress,
                PaymentMethod = orderSummary.PaymentMethod
            };

            await _orderService.AddAsync(newOrder);
            return true;
        }

        public async Task<bool> ValidatePaymentAsync(OrderSummaryViewModel orderSummary)
        {
            // Burada ödeme doğrulama işlemleri yapılır
            return true;
        }

        public async Task<bool> ShipOrderAsync(int orderId)
        {
            // Sipariş kargo bilgileri ile işlenir
            return true;
        }
    }
}
