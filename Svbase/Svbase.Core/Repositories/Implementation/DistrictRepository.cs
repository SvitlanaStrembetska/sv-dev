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
    }
}
