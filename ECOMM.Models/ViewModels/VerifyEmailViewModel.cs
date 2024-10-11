using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class VerifyEmailViewModel
    {

        [Required(ErrorMessage = "Mail girmeniz gerekli")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
