using System.Text.RegularExpressions;

namespace ECOMM.Core.Validation
{
    public class ShippingInfoValidator
    {
        // Dinamik ZipCode doğrulama
        public static bool ValidateZipCode(string country, string zipCode)
        {
            string pattern = country switch
            {
                "USA" => @"^\d{5}(-\d{4})?$",
                "Canada" => @"^[A-Za-z]\d[A-Za-z] \d[A-Za-z]\d$",
                "UK" => @"^[A-Z]{1,2}\d[A-Z\d]? \d[A-Z]{2}$",
                "Germany" => @"^\d{5}$",
                "France" => @"^\d{5}$",
                "Australia" => @"^\d{4}$",
                "India" => @"^\d{6}$",
                "Turkey" => @"^\d{5}$",
                "Brazil" => @"^\d{5}-\d{3}$",
                _ => @"^\d{5}$" // Default pattern (örnek olarak 5 haneli zip kodu)
            };

            var regex = new Regex(pattern);
            return regex.IsMatch(zipCode);
        }
    }
}
