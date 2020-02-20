using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using System.Collections.Generic;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IStreetRepository : IGenericRepository<Street>
    {
        StreetViewModel GetStreetById(int id);
        IEnumerable<StreetSelectModel> GetStreetsForSelecting();
        //IEnumerable<Street> GetStreetsByDistrictId(int id);
        IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetIds(IList<int> streetIds);
        IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetId(int id);
        IEnumerable<ItemFilterModel> GetFilterStreetsByCityIds(IList<int> cityIds);
        IEnumerable<int> GetPersonsIdsByStreetIds(List<int> streetIds);
        IEnumerable<ApartmentFilterModel> GetFilterApartmentsByStreetIds(IList<int> streetIds);
        IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyStreetsBy(int cityId);
    }
}
