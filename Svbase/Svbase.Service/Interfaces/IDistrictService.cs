using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Abstract;

namespace Svbase.Service.Interfaces
{
    public interface IDistrictService : IEntityService<District>
    {
        IEnumerable<DistrictListModel> GetAllDistricts();
        bool CreateDistrictByModel(DistrictCreateModel model);
        DistrictCreateModel GetDistrictModelById(int id);
        DistrictCreateModel GetDistrictById(int id);
        bool EditDistrictByModel(DistrictCreateModel model);
    }
}
