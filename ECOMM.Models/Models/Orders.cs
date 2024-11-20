using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECOMM.Core.Models
{
    public class Orders : BaseModel
    {
        public int OrderId { get; set; }
        public int OrderNumber { get; set; }

        public string UserId { get; set; } // User sınıfındaki Id ile uyumlu hale getirildi
        public User User { get; set; } // Kullanıcı nesnesi

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Siparişin içindeki ürünler
        public DateTime OrderDate { get; set; } // Sipariş tarihi

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } // Toplam tutar

        public string Status { get; set; } // Sipariş durumu (örn. 'Pending', 'Shipped', 'Delivered')
        public string ShippingAddress { get; set; } // Gönderim adresi
        public string PaymentMethod { get; set; } // Ödeme yöntemi,



        public string CustomerName
        {
            get
            {
                return $"{FirstName} {LastName}"; // Concatenate FirstName and LastName with a space
            }
            set
            {
                var names = value.Split(' '); // Split the customer name by space
                if (names.Length > 0) FirstName = names[0];
                if (names.Length > 1) LastName = names[1]; // Assign FirstName and LastName
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public bool SameAddress { get; set; } // Shipping address is the same as billing address
        public bool SaveInfo { get; set; } // Save information for next time

        public string PromoCode { get; set; }

        public List<OrderItem> CartItems { get; set; }



    }
}