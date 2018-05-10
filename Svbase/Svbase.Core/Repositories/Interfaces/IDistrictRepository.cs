using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Enums;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IDistrictRepository : IGenericRepository<District>
    {
        IEnumerable<DistrictListModel> GetAllDistricts();
        DashboardDistrictsModel GetDashboardDistrictsModel();
        IEnumerable<int> GetPersonsIdsByDistrictIds(IEnumerable<int> districtIds);
        IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyDistrictsBy(DistrictType districtType);
        IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyCitiesBy(int districtId);
        //DistrictCreateModel GetDistrictById(int id);
        //IEnumerable<BaseViewModel> GetStretsBaseModelByDistrictIds(IList<int> districtIds);
    }
}
