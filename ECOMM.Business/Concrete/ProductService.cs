using ECOMM.Core.Models;
using ECOMM.Business.Abstract;
using ECOMM.Data.Shared.Abstract;

namespace ECOMM.Business.Concrete
{
    public class ProductService : Service<Product>,IProductService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository) : base(productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            return await _productRepository.AddAsync(product);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            return await _productRepository.UpdateAsync(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}
