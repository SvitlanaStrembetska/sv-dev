﻿using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Service.Abstract;

namespace Svbase.Service.Interfaces
{
    public interface IPersonService : IEntityService<Person>
    {
        IQueryable<PersonSelectionModel> GetPersons();
        PersonViewModel GetPersonById(int id);
        PersonAndFullAddressViewModel GetPersonWithAddressById(int id);
        IList<BaseViewModel> GetDistrictsForFilter();
        IList<BaseViewModel> GetWorksBaseViewModels();
        IList<BaseViewModel> GetCitiesBaseViewModels();
        IList<BaseViewModel> GetStreetsForFilter();
        //IEnumerable<BaseViewModel> GetStretsBaseModelByStreetSearchFilter(StreetSearchFilterModel filter);
        IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetIds(IList<int> streetIds);
        IEnumerable<BaseViewModel> GetFlatsBaseModelByApatrmentIds(IList<int> apartmentIds);
        bool CreatePersonByModel(PersonAndFullAddressViewModel model, Flat flat);
        IQueryable<PersonSelectionModel> SearchPersonsByFilter(FilterFileImportModel filter);
        IEnumerable<ItemFilterModel> GetFilterStreetsByCityIds(IList<int> cityIds);
    }
}
