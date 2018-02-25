using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        public CityRepository(ApplicationDbContext context)
            : base(context) { }

        public IEnumerable<CityCreateModel> GetCities()
        {
            var cities = DbSet.Select(x => new CityCreateModel
            {
                Id = x.Id,
                Name = x.Name,
                CanDelete = !x.Streets.Any()
            });
            return cities;
        }
        public IEnumerable<CityViewModel> GetAllCities()
        {
            var cities = DbSet.Select(x => new CityViewModel
            {
                Id = x.Id,
                Name = x.Name,
            });
            return cities;
        }

        public CityViewModel GetCityById(int id)
        {
            var city = DbSet.Select(x => new CityViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Streets = x.Streets.Select(s => new StreetCreateModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    CanDelete = !s.Apartments.Any()
                })
            })
            .FirstOrDefault(x => x.Id == id);
            return city;
        }

        public IEnumerable<BaseViewModel> GetStretsBaseModelByCityIds(IList<int> cityIds)
        {
            if(cityIds == null) return new List<BaseViewModel>();
            var cities = DbSet.Where(x => cityIds.Contains(x.Id));
            if (!cities.Any()) return new List<BaseViewModel>();
            var streetsLists = cities
                .Select(x => x.Streets
                    .Select(s => new BaseViewModel
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList()
                ).ToList();
            var streets = new List<BaseViewModel>();
            streets = streetsLists
                .Aggregate(streets, (current, items) => current.Union(items).ToList());//Union arrays
            streets = streets.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();//Distinct by field
            return streets;
        }

        public IEnumerable<BaseViewModel> GetStreetsBaseModelByCityId(int id)
        {
            var city = DbSet.FirstOrDefault(x => x.Id == id);
            if(city == null) return new List<BaseViewModel>();
            var streets = city.Streets.Select(x => new BaseViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
            return streets;
        }
    }
}
