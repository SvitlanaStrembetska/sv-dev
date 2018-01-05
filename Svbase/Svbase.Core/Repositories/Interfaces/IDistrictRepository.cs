using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IDistrictRepository : IGenericRepository<District>
    {
        IEnumerable<DistrictListModel> GetAllDistricts();
        DistrictCreateModel GetDistrictById(int id);
        IEnumerable<BaseViewModel> GetStretsBaseModelByDistrictIds(IList<int> districtIds);
    }
}
