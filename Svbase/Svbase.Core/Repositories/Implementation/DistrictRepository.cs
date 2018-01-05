using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
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

        public DistrictCreateModel GetDistrictById(int id)
        {
            var district = DbSet.Select(x => new DistrictCreateModel
            {
                Id = x.Id,
                Name = x.Name,
                DistrictType = x.DistrictType,
                Streets = x.Streets.Select(s => new StreetSelectModel
                {
                    Id = s.Id,
                    Name = s.Name,
                    CityName = s.City.Name,
                    IsChecked = true
                }).ToList()
            }).FirstOrDefault(x => x.Id == id);

            return district;
        }

        public IEnumerable<BaseViewModel> GetStretsBaseModelByDistrictIds(IList<int> districtIds)
        {
            if (districtIds == null) return new List<BaseViewModel>();
            var districts = DbSet.Where(x => districtIds.Contains(x.Id));
            if(!districts.Any()) return new List<BaseViewModel>();
            var streetsLists = districts
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
    }
}
