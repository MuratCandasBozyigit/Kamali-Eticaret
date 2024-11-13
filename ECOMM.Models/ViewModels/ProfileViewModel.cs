using ECOMM.Core.Models;

namespace ECOMM.Core.ViewModels
{
    public class ProfileViewModel
    {
      //  public string UserName { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public List<Orders> UserOrders { get; set; } = new List<Orders>();

    }
}
