using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Enums;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class DistrictRepository : GenericRepository<District>, IDistrictRepository
    {
        public DistrictRepository(ApplicationDbContext context)
            : base(context) { }

        public IEnumerable<DistrictListModel> GetAllDistricts()
        {
            var districts = DbSet.Select(x => new DistrictListModel
            {
                Id = x.Id,
                Name = x.Name,
                DistrictType = x.DistrictType
            });
            return districts;
        }

        private int GetPersonsCountByApartments(ICollection<Apartment> apartments)
        {
            var sum = apartments.Sum(a => a.Flats.Sum(f => f.Persons.Count));
            return sum;
        }

        public DashboardDistrictsModel GetDashboardDistrictsModel()
        {
            var districts = DbSet
                .Select(x => new DashboardDistrictItemModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    DistrictType = x.DistrictType,
                }).ToList();


            var apartmentsList = DbSet.Select(x => x.Apartments).ToList();

            if (apartmentsList.Count == districts.Count)
            {
                for (var i = 0; i < apartmentsList.Count; i++)
                {
                    districts[i].PersonsCount = GetPersonsCountByApartments(apartmentsList[i]);
                }
            }

            var constituencyDistrcits = districts
                .Where(x => x.DistrictType == DistrictType.Сonstituency)
                .Select(x => new DashboardItemModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PersonsCount = x.PersonsCount
                });

            var customDistricts = districts
                .Where(x => x.DistrictType == DistrictType.Custom)
                .Select(x => new DashboardItemModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    PersonsCount = x.PersonsCount
                });

            var dashboardDistrictsModel = new DashboardDistrictsModel
            {
                ConstituencyDistrcits = constituencyDistrcits,
                CustomDistricts = customDistricts
            };
            return dashboardDistrictsModel;
        }

        public IEnumerable<int> GetPersonsIdsByDistrictIds(IEnumerable<int> districtIds)
        {
            if(districtIds == null) return new List<int>();
            var districts = DbSet.Where(x => districtIds.ToList().Contains(x.Id)).Select(x => x);
            if(!districts.Any())
                return new List<int>();

            var districtApartments = districts.Select(x => x.Apartments).ToList();
            if(!districtApartments.Any())
                return new List<int>();

            var apartments = new List<Apartment>();
            foreach (var districtApartment in districtApartments)
            {
                apartments.AddRange(districtApartment);
            }

            apartments = apartments.Distinct().ToList();
            if(!apartments.Any())
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

        public IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyDistrictsBy(DistrictType districtType)
        {
            var districts = DbSet
                .Where(x => x.DistrictType == districtType)
                .Select(x => new DistrictPanelBodyItemModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    DistrictPanelBodyItemType = DistrictPanelBodyItemType.District
                });
            return districts;
        }

        public IEnumerable<BaseViewModel> GetDistrictsByType(DistrictType districtType)
        {
            var districts = DbSet
                .Where(x => x.DistrictType == districtType)
                .Select(x => new BaseViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                });
            return districts;
        }


        public IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyCitiesBy(int districtId)
        {
            var district = DbSet.FirstOrDefault(x => x.Id == districtId);
            if (district == null)
                return new List<DistrictPanelBodyItemModel>();
            var cities = district.Apartments.Select(x => x.Street.City).Distinct().ToList();
            var districtPanelBodyCities = cities.Select(x => new DistrictPanelBodyItemModel
            {
                Id = x.Id,
                Name = x.Name,
                DistrictPanelBodyItemType = DistrictPanelBodyItemType.City
            });
            return districtPanelBodyCities;
        }


        //public DistrictCreateModel GetDistrictById(int id)
        //{
        //    var district = DbSet.Select(x => new DistrictCreateModel
        //    {
        //        Id = x.Id,
        //        Name = x.Name,
        //        DistrictType = x.DistrictType,
        //        Streets = x.Streets.Select(s => new StreetSelectModel
        //        {
        //            Id = s.Id,
        //            Name = s.Name,
        //            CityName = s.City.Name,
        //            IsChecked = true
        //        }).ToList()
        //    }).FirstOrDefault(x => x.Id == id);

        //    return district;
        //}

        //public IEnumerable<BaseViewModel> GetStretsBaseModelByDistrictIds(IList<int> districtIds)
        //{
        //    if (districtIds == null) return new List<BaseViewModel>();
        //    var districts = DbSet.Where(x => districtIds.Contains(x.Id));
        //    if(!districts.Any()) return new List<BaseViewModel>();
        //    var streetsLists = districts
        //        .Select(x => x.Streets
        //            .Select(s => new BaseViewModel
        //                {
        //                    Id = s.Id,
        //                    Name = s.Name
        //                }).ToList()
        //        ).ToList();
        //    var streets = new List<BaseViewModel>();
        //    streets = streetsLists
        //        .Aggregate(streets, (current, items) => current.Union(items).ToList());//Union arrays
        //    streets = streets.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();//Distinct by field
        //    return streets;
        //}
    }
}
