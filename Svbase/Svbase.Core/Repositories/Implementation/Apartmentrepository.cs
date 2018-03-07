using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class ApartmentRepository : GenericRepository<Apartment>, IApartmentRepository
    {
        public ApartmentRepository(ApplicationDbContext context)
            : base(context) { }

        public ApartmentViewModel GetById(int id)
        {
            var apartment = DbSet.Select(x => new ApartmentViewModel
            {
                Id = x.Id,
                Name = x.Name,
                StreetId = x.StreetId,
                Flats = x.Flats.Select(f => new FlatCreateModel
                {
                    Id = f.Id,
                    Name = f.Number,
                    ApartmentId = f.ApartmentId,
                    CanDelete = !f.Persons.Any()
                })
            }).FirstOrDefault(x => x.Id == id);
            return apartment;
        }

        public IEnumerable<BaseViewModel> GetFlatBaseModelByApartmentIds(IList<int> apartmentIds)
        {
            if (apartmentIds == null) return new List<BaseViewModel>();
            var apartments = DbSet.Where(x => apartmentIds.Contains(x.Id));
            if (!apartments.Any()) return new List<BaseViewModel>();

            var flatsLists = apartments
                .Select(x => x.Flats
                    .Select(s => new BaseViewModel
                    {
                        Id = s.Id,
                        Name = s.Number
                    }).ToList()
                ).ToList();

            var flats = new List<BaseViewModel>();
            flats = flatsLists
                .Aggregate(flats, (current, items) => current.Union(items).ToList());//Union arrays
            flats = flats.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();//Distinct by field
            return flats;
        }

        public IEnumerable<BaseViewModel> GetFlatsBaseModelByApartmentId(int id)
        {
            var apartment = DbSet.FirstOrDefault(x => x.Id == id);
            if (apartment == null) return new List<BaseViewModel>();
            var flats = apartment.Flats.Select(x => new BaseViewModel
            {
                Id = x.Id,
                Name = x.Number
            });
            return flats;
        }

        public IEnumerable<int> GetPersonsIdsByApartmentIds(List<int> apartmentIds)
        {
            if (apartmentIds == null || !apartmentIds.Any())
                return new List<int>();

            var apartments = DbSet
                .Where(x => apartmentIds.Contains(x.Id))
                .Select(x => x);

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

        public IEnumerable<CityFlatFilterModel> GetFilterFlatsByApartmentIds(IList<int> apartmentIds)
        {
            if (apartmentIds == null || !apartmentIds.Any())
            {
                return new List<CityFlatFilterModel>();
            }

            var apartmentsDb = DbSet
                .Include(x => x.Street.City)
                .Where(x => apartmentIds.Contains(x.Id));

            var streetsDb = apartmentsDb
                .Select(x => x.Street)
                .Distinct();

            var cityIds = streetsDb.Select(x => x.CityId).Distinct().ToList();
            var streetList = new List<List<Street>>();
            foreach (var cityId in cityIds)
            {
                var streetsByCityId = streetsDb.Where(x => x.CityId == cityId).ToList();
                streetList.Add(streetsByCityId);
            }

            var filterItems = new List<CityFlatFilterModel>();
            foreach (var streets in streetList)
            {
                var item = new CityFlatFilterModel
                {
                    City = new BaseViewModel
                    {
                        Id = streets.FirstOrDefault()?.CityId ?? 0,
                        Name = streets.FirstOrDefault()?.City?.Name
                    },
                    Streets = streets.Select(x => new StreetFlatFilterModel
                    {
                        Street = new BaseViewModel
                        {
                            Id = x.Id,
                            Name = x.Name
                        },
                        Apartments = x.Apartments
                            .Where(a => apartmentIds.Contains(a.Id))
                            .Select(a => new ApartmentFlatModel
                        {
                            Apartment = new BaseViewModel
                            {
                                Id = a.Id,
                                Name = a.Name
                            },
                            Flats = a.Flats.Select(f => new BaseViewModel
                            {
                                Id = f.Id,
                                Name = f.Number
                            }).ToList()
                        }).ToList()
                    }).ToList()
                };
                filterItems.Add(item);
            }
            return filterItems;
        }

    }
}
