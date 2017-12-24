using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Core.Repositories.Interfaces;
using Svbase.Service.Abstract;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class CityService : EntityService<ICityRepository,City>, ICityService
    {
        public CityService(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            :base(unitOfWork, repositoryManager, repositoryManager.Cities)
        {
            
        }

        public IEnumerable<CityCreateModel> GetCities()
        {
            var cities = RepositoryManager.Cities.GetCities();
            return cities;
        }

        public CityViewModel GetCityById(int id)
        {
            var city = RepositoryManager.Cities.GetCityById(id);
            return city;
        }
    }
}
