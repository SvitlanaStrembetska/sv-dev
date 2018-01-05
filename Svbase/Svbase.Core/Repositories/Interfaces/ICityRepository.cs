using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface ICityRepository : IGenericRepository<City>
    {
        IEnumerable<CityCreateModel> GetCities();
        CityViewModel GetCityById(int id);
        IEnumerable<BaseViewModel> GetStretsBaseModelByCityIds(IList<int> cityIds);
        IEnumerable<BaseViewModel> GetStreetsBaseModelByCityId(int id);
    }
}
