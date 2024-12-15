using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Core.ViewModels;
using ECOMM.Data.Shared.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECOMM.Business.Concrete
{
    public class SubCategoryService : Service<SubCategory>, ISubCategoryService
    {
        private readonly IRepository<SubCategory> _subCategoryRepository;

        public SubCategoryService(IRepository<SubCategory> subCategoryRepository) : base(subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository;
        }

        public async Task<SubCategory> AddAsync(SubCategory entity)
        {
            // Implement the add logic
            return await _subCategoryRepository.AddAsync(entity);
        }

        public async Task<List<SubCategory>> AddRangeAsync(List<SubCategory> entities)
        {
            // Implement the add range logic
            return await _subCategoryRepository.AddRangeAsync(entities);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            // Implement the delete logic
            return await _subCategoryRepository.DeleteAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid guid)
        {
            // Implement the delete logic for Guid
            return await _subCategoryRepository.DeleteAsync(guid);
        }
        public async Task<IEnumerable<SubCategory>> GetAllIncludingCategoryAsync()
        {
            // Alt kategorileri ve ilişkili kategorileri getir
            return await _subCategoryRepository.GetAllIncluding(c => c.Category);
        }

        public async Task<IEnumerable<SubCategory>> GetAllAsync()
        {
            // Implement the get all logic
            return await _subCategoryRepository.GetAllAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetAllAsync(Expression<Func<SubCategory, bool>> predicate)
        {
            // Implement the get all with predicate logic
            return await _subCategoryRepository.GetAllAsync(predicate);
        }

        public async Task<SubCategory> GetByIdAsync(int id)
        {
            // Implement the get by id logic
            return await _subCategoryRepository.GetByIdAsync(id);
        }

        public async Task<SubCategory> GetByIdAsync(Guid guid)
        {
            // Implement the get by guid logic
            return await _subCategoryRepository.GetByIdAsync(guid);
        }

        public async Task<SubCategory> GetFirstOrDefaultAsync(Expression<Func<SubCategory, bool>> predicate)
        {
            // Implement the get first or default logic
            return await _subCategoryRepository.GetFirstOrDefaultAsync(predicate);
        }

        public async Task<SubCategory> UpdateAsync(SubCategory entity)
        {
            // Implement the update logic
            return await _subCategoryRepository.UpdateAsync(entity);
        }

        public async Task<IEnumerable<SubCategory>> GetByCategoryIdAsync(int categoryId)
        {
            // Await the GetAllAsync() task and then apply LINQ methods on the result.
            var subCategories = await _subCategoryRepository.GetAllAsync();
            return subCategories.Where(s => s.CategoryId == categoryId).ToList();
        }


    }
}
