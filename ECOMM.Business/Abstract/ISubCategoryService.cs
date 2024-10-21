using ECOMM.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Business.Abstract
{
    public interface ISubCategoryService:IService<SubCategory>
    {

        Task<IEnumerable<SubCategory>> GetAllIncludingCategoryAsync();

    }
}
