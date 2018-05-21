using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Enums;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class StreetRepository : GenericRepository<Street>, IStreetRepository
    {
        public StreetRepository(ApplicationDbContext context)
            : base(context) { }

        public StreetViewModel GetStreetById(int id)
        {
            var street = DbSet.OrderBy(y => y.Name).Select(x => new StreetViewModel
            {
                Id = x.Id,
                Name = x.Name,
                Pseudonym = x.Pseudonym,
                CityId = x.CityId,
                CanDelete = !x.Apartments.Any(),
                Apartments = x.Apartments.Select(a => new ApartmentCreateModel
                {
                    Id = a.Id,
                    Name = a.Name,
                    CanDelete = !a.Flats.Any()
                }).OrderBy(y => y.Name.Length)
                  .ThenBy(y => y.Name)
            }).FirstOrDefault(x => x.Id == id);
            return street;
        }

        public IEnumerable<StreetSelectModel> GetStreetsForSelecting()
        {
            var streets = DbSet.OrderBy(y => y.Name).Select(x => new StreetSelectModel
            {
                Id = x.Id,
                Name = x.Name,
                Pseudonym = x.Pseudonym,
                CityName = x.City.Name,
                IsChecked = false
            });

            return streets;
        }

        //public IEnumerable<Street> GetStreetsByDistrictId(int id)
        //{
        //    var streets = DbSet.Where(x => x.Districts.Select(d => d.Id).Contains(id));
        //    return streets;
        //}

        public IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetIds(IList<int> streetIds)
        {
            if (streetIds == null) return new List<BaseViewModel>();
            var streets = DbSet.Where(x => streetIds.Contains(x.Id));
            if (!streets.Any()) return new List<BaseViewModel>();

            var apartmentsLists = streets
                .Select(x => x.Apartments
                    .Select(s => new BaseViewModel
                    {
                        Id = s.Id,
                        Name = s.Name
                    }).ToList()
                ).ToList();

            var apartments = new List<BaseViewModel>();
            apartments = apartmentsLists
                .Aggregate(apartments, (current, items) => current.Union(items).ToList());//Union arrays
            apartments = apartments.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();//Distinct by field
            return apartments;
        }

        public IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetId(int id)
        {
            var street = DbSet.FirstOrDefault(x => x.Id == id);
            if (street == null) return new List<BaseViewModel>();
            var apartments = street.Apartments.Select(x => new BaseViewModel
            {
                Id = x.Id,
                Name = x.Name
            });
            return apartments;
        }

        public IEnumerable<ItemFilterModel> GetFilterStreetsByCityIds(IList<int> cityIds)
        {
            if (cityIds == null || !cityIds.Any())
            {
                return new List<ItemFilterModel>();
            }

            var streetsDb = DbSet
                .Where(x => cityIds.Contains(x.CityId))
                .Include(x => x.City);

            var streetList = new List<List<Street>>();
            foreach (var cityId in cityIds)
            {
                var streetsById = streetsDb.Where(x => x.CityId == cityId).ToList();
                streetList.Add(streetsById);
            }
            var filterItems = new List<ItemFilterModel>();
            foreach (var streets in streetList)
            {
                var item = new ItemFilterModel
                {
                    ParentId = streets.FirstOrDefault()?.CityId ?? 0,
                    ParentName = streets.FirstOrDefault()?.City?.Name,
                    Items = streets.Select(x => new StreetBaseViewModel
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Pseudonym = x.Pseudonym
                        })
                        .ToList()
                };
                filterItems.Add(item);
            }

            return filterItems;
        }

        public IEnumerable<ApartmentFilterModel> GetFilterApartmentsByStreetIds(IList<int> streetIds)
        {
            if (streetIds == null || !streetIds.Any())
            {
                return new List<ApartmentFilterModel>();
            }

            var streetsDb = DbSet
              .Where(x => streetIds.Contains(x.Id))
              .Include(x => x.City)
              .Include(x => x.Apartments);

            var cityIds = streetsDb.Select(x => x.CityId).Distinct().ToList();
            var streetList = new List<List<Street>>();
            foreach (var cityId in cityIds)
            {
                var streetsByCityId = streetsDb.Where(x => x.CityId == cityId).ToList();
                streetList.Add(streetsByCityId);
            }

            var filterItems = new List<ApartmentFilterModel>();
            foreach (var streets in streetList)
            {
                var item = new ApartmentFilterModel
                {
                    City = new BaseViewModel
                    {
                        Id = streets.FirstOrDefault()?.CityId ?? 0,
                        Name = streets.FirstOrDefault()?.City?.Name
                    },
                    Streets = streets.OrderBy(y => y.Name).Select(x => new ItemFilterModel
                    {
                        ParentId = x.Id,
                        ParentName = x.Name,
                        Items = x.Apartments.Select(a => new StreetBaseViewModel
                        {
                            Id = a.Id,
                            Name = a.Name
                        }).ToList()
                    }).ToList()
                };
                filterItems.Add(item);
            }
            return filterItems;
        }

        public IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyStreetsBy(int cityId)
        {
            var streets = DbSet
                .Where(x => x.CityId == cityId)
                .Select(x => new DistrictPanelBodyItemModel
            {
                Id = x.Id,
                Name = x.Name,
                DistrictPanelBodyItemType = DistrictPanelBodyItemType.Street
            });
            return streets;
        }

        public IEnumerable<int> GetPersonsIdsByStreetIds(List<int> streetIds)
        {
            if (streetIds == null || !streetIds.Any())
                return new List<int>();

            var streets = DbSet.Where(x => streetIds.Contains(x.Id)).Select(x => x);
            if (!streets.Any())
                return new List<int>();

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
