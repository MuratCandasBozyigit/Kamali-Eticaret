using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Core.ViewModels
{
    public class CommentViewModel
    {
        public string Content { get; set; }
        public string Author { get; set; }
        public bool IsApproved { get; set; } = false;
        public DateTime DateCommented { get; set; }
    }

}
