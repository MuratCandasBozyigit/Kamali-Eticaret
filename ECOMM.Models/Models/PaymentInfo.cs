using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using ECOMM.Core.Enums;

namespace ECOMM.Core.Models
{
    public class PaymentInfo
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Orders Order { get; set; }

        [Required]
        [EnumDataType(typeof(PaymentMethodEnum))]
        public PaymentMethodEnum PaymentMethod { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount paid must be greater than 0.")]
        public decimal AmountPaid { get; set; }

        [Required]
        public DateTime PaymentDate { get; set; }

        public string PaymentStatus { get; set; }
    }


}
