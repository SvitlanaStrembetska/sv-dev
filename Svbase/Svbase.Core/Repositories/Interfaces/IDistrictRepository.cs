using Svbase.Core.Data.Entities;
using Svbase.Core.Enums;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using System.Collections.Generic;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IDistrictRepository : IGenericRepository<District>
    {
        IEnumerable<DistrictListModel> GetAllDistricts();
        DashboardDistrictsModel GetDashboardDistrictsModel();
        IEnumerable<int> GetPersonsIdsByDistrictIds(IEnumerable<int> districtIds);
        IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyDistrictsBy(DistrictType districtType);
        IEnumerable<BaseViewModel> GetDistrictsByType(DistrictType districtType);
        IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyCitiesBy(int districtId);
    }
}
