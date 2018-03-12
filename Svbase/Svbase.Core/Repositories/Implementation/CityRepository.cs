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
            var cities = DbSet.OrderBy(y => y.Name).Select(x => new CityCreateModel
            {
                Id = x.Id,
                Name = x.Name,
                CanDelete = !x.Streets.Any()
            });
            return cities;
        }
        public IEnumerable<CityViewModel> GetAllCities()
        {
            var cities = DbSet.OrderBy(y => y.Name).Select(x => new CityViewModel
            {
                Id = x.Id,
                Name = x.Name,
            });
            return cities;
        }

        public CityViewModel GetCityById(int id)
        {
            var city = DbSet.OrderBy(y => y.Name).Select(x => new CityViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Streets = x.Streets.OrderBy(y => y.Name).Select(s => new StreetCreateModel
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
            var cities = DbSet.OrderBy(y => y.Name).Where(x => cityIds.Contains(x.Id));
            if (!cities.Any()) return new List<BaseViewModel>();
            var streetsLists = cities.OrderBy(y => y.Name)
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
            streets = streets.OrderBy(y => y.Name).GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();//Distinct by field
            return streets;
        }

        public IEnumerable<BaseViewModel> GetStreetsBaseModelByCityId(int id)
        {
            var city = DbSet.FirstOrDefault(x => x.Id == id);
            if(city == null) return new List<BaseViewModel>();
            var streets = city.Streets.OrderBy(y => y.Name).Select(x => new BaseViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
            return streets;
        }

        public IEnumerable<int> GetPersonsIdsByCityIds(List<int> cityIds)
        {
            if(cityIds == null || !cityIds.Any())
                return new List<int>();

            var cities = DbSet.Where(x => cityIds.Contains(x.Id)).Select(x => x);
            if (!cities.Any())
                return new List<int>();

            var cityStreets = cities.Select(x => x.Streets).ToList();
            if (!cityStreets.Any())
                return new List<int>();

            var streets = new List<Street>();
            foreach (var street in cityStreets)
            {
                streets.AddRange(street);
            }

            streets = streets.Distinct().ToList();
            if (!streets.Any())
                return new List<int>();

            var streetApartments = streets.Select(x => x.Apartments).ToList();
            if (!streetApartments.Any())
                return new List<int>();

            var apartments = new List<Apartment>();
            foreach (var streetApartmentApartment in streetApartments)
            {
                apartments.AddRange(streetApartmentApartment);
            }

            apartments = apartments.Distinct().ToList();
            if (!apartments.Any())
                return new List<int>();

            var apartmentFlats = apartments.Select(x => x.Flats).ToList();
            if (!apartmentFlats.Any())
                return new List<int>();

            var flats = new List<Flat>();
            foreach (var apartmentFlat in apartmentFlats)
            {
                flats.AddRange(apartmentFlat);
            }
            if (!flats.Any())
                return new List<int>();

            var flatPersons = flats.Select(x => x.Persons);
            var persons = new List<Person>();
            foreach (var flatPerson in flatPersons)
            {
                persons.AddRange(flatPerson);
            }
            var personsIds = persons.Select(p => p.Id);
            return personsIds;
        }
    }
}
