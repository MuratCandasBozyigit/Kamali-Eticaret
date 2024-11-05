using ECOMM.Business.Abstract;
using ECOMM.Core.Models;
using ECOMM.Data.Shared.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECOMM.Business.Concrete
{
    public class FavouritesService : Service<Favourites>, IFavouritesService
    {
        private readonly IRepository<Favourites> _favouritesRepository;

        public FavouritesService(IRepository<Favourites> favouritesRepository):base(favouritesRepository)
        {
            _favouritesRepository = favouritesRepository;
        }
        public async Task<Favourites> GetByIdAsync(int id)
        {
            return await _favouritesRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Favourites>> GetAllAsync()
        {
            return await _favouritesRepository.GetAllAsync();
        }

        public async Task<Favourites> AddAsync(Favourites favourites)
        {
            return await _favouritesRepository.AddAsync(favourites);
        }

        public async Task<Favourites> UpdateAsync(Favourites favourites)
        {
            return await _favouritesRepository.UpdateAsync(favourites);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _favouritesRepository.DeleteAsync(id);
        }
    }
}
