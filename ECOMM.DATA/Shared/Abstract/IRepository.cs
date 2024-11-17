using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ECOMM.Core.Models;

namespace ECOMM.Data.Shared.Abstract
{
    public interface IRepository<T> where T : BaseModel
    {
        IQueryable<T> Query();
        Task<IEnumerable<T>> GetAllAsync(); // Tüm verileri getirir
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate); // Şarta göre verileri getirir
        Task<T> GetByIdAsync(int id); // ID ile veri getirir
        Task<T> GetByIdAsync(Guid guid); // Guid ile veri getirir
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate); // İlk eşleşen kaydı getirir
        Task<T> AddAsync(T entity); // Yeni veri ekler
        Task<List<T>> AddRangeAsync(List<T> entities); // Toplu veri ekler
        Task<T> UpdateAsync(T entity); // Veri günceller
        Task<bool> DeleteAsync(int id); // ID ile veri siler
        Task<bool> DeleteAsync(Guid guid); // Guid ile veri siler
        Task SaveAsync(); // Değişiklikleri kaydeder
        Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
        Task<Category> GetCategoryByIdAsync(int categoryId);
    }
}
