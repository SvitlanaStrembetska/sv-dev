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

        public CityViewModel GetCityById(int id)
        {
            var city = DbSet.Select(x => new CityViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Streets = x.Streets.Select(s => new StreetCreateModel
                {
                    Id = s.Id,
                    Name = s.Name
                })
            })
            .FirstOrDefault(x => x.Id == id);
            return city;
        }
    }
}
