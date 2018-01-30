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

        public DashboardDistrictsModel GetDashboardDistrictsModel()
        {

            var districts = DbSet
                .Select(x => new DashboardDistrictItemModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    DistrictType = x.DistrictType,
                    PersonsCount = x.Apartments.Sum(a => a.Flats.Sum(f => f.Persons.Count))
                });

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
