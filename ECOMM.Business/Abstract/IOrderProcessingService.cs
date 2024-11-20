using System.Threading.Tasks;
using ECOMM.Core.ViewModels;

namespace ECOMM.Business.Abstract
{
    public interface IOrderProcessingService
    {
        Task<bool> ProcessOrderAsync(OrderSummaryViewModel orderSummary); // Siparişi işle
        Task<bool> ValidatePaymentAsync(OrderSummaryViewModel orderSummary); // Ödeme doğrulama
        Task<bool> ShipOrderAsync(int orderId); // Siparişi gönder
    }
}
