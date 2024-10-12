using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ECOMM.Core.Models;

namespace ECOMM.Business.Abstract
{
    public interface IService<T> where T : BaseModel
    {
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
    }
}
