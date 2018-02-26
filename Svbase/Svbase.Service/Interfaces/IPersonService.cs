using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Abstract;

namespace Svbase.Service.Interfaces
{
    public interface IPersonService : IEntityService<Person>
    {
        IEnumerable<PersonViewModel> GetPersons();
        IEnumerable<PersonViewModel> GetPersonsByBeneficiariesId(int beneficiaryId);
        PersonViewModel GetPersonById(int id);

        IList<BaseViewModel> GetDistrictsForFilter();
        IList<BaseViewModel> GetCitiesBaseViewModels();
        IList<BaseViewModel> GetStreetsForFilter();
        //IEnumerable<BaseViewModel> GetStretsBaseModelByStreetSearchFilter(StreetSearchFilterModel filter);
        IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetIds(IList<int> streetIds);
        IEnumerable<BaseViewModel> GetFlatsBaseModelByApatrmentIds(IList<int> apartmentIds);
        bool CreatePersonByModel(PersonViewModel model);
        List<PersonViewModel> SearchPersonsByFilter(FilterSearchModel filter);
        IEnumerable<ItemFilterModel> GetFilterStreetsByCityIds(IList<int> cityIds);
    }
}
