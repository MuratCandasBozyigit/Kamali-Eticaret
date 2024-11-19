using ECOMM.Core.Models;
using ECOMM.Data.Shared.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECOMM.Data.Shared.Concrete
{
    public class Repository<T> : IRepository<T> where T : BaseModel
    {
        private readonly ApplicationDBContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(ApplicationDBContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public IQueryable<T> Query()
        {
            return _dbSet.AsQueryable();
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
            await SaveAsync();
            return entity;
        }

        public async Task<IEnumerable<T>> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _dbSet;

            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }

            return await query.ToListAsync();
        }

        public async Task<List<T>> AddRangeAsync(List<T> entities)
        {
            if (entities == null || entities.Count == 0) throw new ArgumentNullException(nameof(entities));

            await _dbSet.AddRangeAsync(entities);
            await SaveAsync();
            return entities;
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            _dbSet.Update(entity);
            await SaveAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            var entity = await _dbSet.FindAsync(guid);
            if (entity == null) return false;

            _dbSet.Remove(entity);
            await SaveAsync();
            return true;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
        }

        public async Task<T> GetByIdAsync(Guid guid)
        {
            return await _dbSet.FindAsync(guid) ?? throw new KeyNotFoundException($"Entity with guid {guid} not found.");
        }

        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate) ?? throw new KeyNotFoundException("Entity not found.");
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        // Örnek olarak eklenmiş bir özel metod (Post ID'ye göre yorumları getirir):
        public async Task<IEnumerable<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _context.Comments.Where(c => c.ProductId == postId).ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Categories.FindAsync(categoryId); // Kategoriyi ID'ye göre buluyoruz
        }

        //public async Task<Orders> CreateOrderAsync(Orders order, User user)
        //{
        //    // Kullanıcıyla ilişkilendirme (örnek)
        //    order.UserId = user.Id; // UserId ilişkilendirildi
        //    order.OrderDate = DateTime.UtcNow; // Sipariş tarihi atanıyor

        //    var result = await _context.Set<Orders>().AddAsync(order);
        //    await _context.SaveChangesAsync();

        //    return result.Entity; // Kaydedilen sipariş döndürülüyor
        //}

    }
}
