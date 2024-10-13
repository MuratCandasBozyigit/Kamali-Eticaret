using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Data.Shared.Abstract;

namespace ECOMM.Business.Concrete
{
    public class Service<T> : IService<T> where T : BaseModel
    {
        private readonly IRepository<T> _repository;

        public Service(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.GetAllAsync(predicate);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<T> GetByIdAsync(Guid guid)
        {
            return await _repository.GetByIdAsync(guid);
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.GetFirstOrDefaultAsync(predicate);
        }

        public async Task<T> AddAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            return await _repository.AddRangeAsync(entities);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            return await _repository.DeleteAsync(guid);
        }
    }
}
