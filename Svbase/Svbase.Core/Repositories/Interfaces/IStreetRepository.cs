using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IStreetRepository : IGenericRepository<Street>
    {
        StreetViewModel GetStreetById(int id);
        IEnumerable<StreetSelectModel> GetStreetsForSelecting();
        //IEnumerable<Street> GetStreetsByDistrictId(int id);
        IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetIds(IList<int> streetIds);
        IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetId(int id);
    }
}
