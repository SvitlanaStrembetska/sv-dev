using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Core.Repositories.Interfaces;
using Svbase.Service.Abstract;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class PersonService : EntityService<IPersonRepository, Person>, IPersonService
    {
        public PersonService(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
            : base(unitOfWork, repositoryManager, repositoryManager.Persons)
        {

        }

        public IQueryable<PersonSelectionModel> GetPersons()
        {
            return RepositoryManager.Persons.GetPersons();
        }

        public PersonViewModel GetPersonById(int id)
        {
            return RepositoryManager.Persons.GetPersonById(id); 
        }

        public PersonAndFullAddressViewModel GetPersonWithAddressById(int id)
        {
            return RepositoryManager.Persons.GetPersonWithAddressById(id); 
        }

        public IList<BaseViewModel> GetDistrictsForFilter()
        {
            var districts = RepositoryManager.Districts.GetAllDistricts().Select(x => new BaseViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return districts;
        }

        public IList<BaseViewModel> GetWorksBaseViewModels()
        {
            return RepositoryManager.Works.GetAll().Select(x => new BaseViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        public IList<BaseViewModel> GetCitiesBaseViewModels()
        {
            var streets = RepositoryManager.Cities.GetCities().Select(x => new BaseViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return streets;
        }

        public IList<BaseViewModel> GetStreetsForFilter()
        {
            var streets = RepositoryManager.Streets.GetStreetsForSelecting().Select(x => new BaseViewModel
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return streets;
        }

        //public IEnumerable<BaseViewModel> GetStretsBaseModelByStreetSearchFilter(StreetSearchFilterModel filter)
        //{
        //    if (filter == null) return new List<BaseViewModel>(); ;
        //    var districtStreets = RepositoryManager.Districts.GetStretsBaseModelByDistrictIds(filter.DistrictIds);
        //    var cityStreets = RepositoryManager.Cities.GetStretsBaseModelByCityIds(filter.CityIds);
        //    var streets = districtStreets.Union(cityStreets).ToList();
        //    streets = streets.GroupBy(x => x.Id).Select(x => x.FirstOrDefault()).ToList();//Distinct by field
        //    return streets;
        //}

        public IEnumerable<BaseViewModel> GetApartmentsBaseModelByStreetIds(IList<int> streetIds)
        {
            var apartments = RepositoryManager.Streets.GetApartmentsBaseModelByStreetIds(streetIds);
            return apartments;
        }

        public IEnumerable<ItemFilterModel> GetFilterStreetsByCityIds(IList<int> cityIds)
        {
            var streets = RepositoryManager.Streets.GetFilterStreetsByCityIds(cityIds);
            return streets;
        }

        public IQueryable<PersonSelectionModel> SearchPersonsByFields(PersonSearchModel searchFields)
        {
            return RepositoryManager.Persons.SearchPersonsByFields(searchFields);
        }

        public IQueryable<PersonSelectionModel> SearchDublicateByFirstAndLastName()
        {
            return RepositoryManager.Persons.SearchDublicateByFirstAndLastName();
        }

        public IEnumerable<BaseViewModel> GetFlatsBaseModelByApatrmentIds(IList<int> apartmentIds)
        {
            var flats = RepositoryManager.Apartments.GetFlatBaseModelByApartmentIds(apartmentIds);
            return flats;
        }

        public bool CreatePersonByModel(PersonAndFullAddressViewModel model, Flat flat)
        {
            if (model == null)
            {
                return false;
            }

            var newPerson = model.Update(new Person());

            if (model.Beneficiaries != null && model.Beneficiaries.Any())
            {
                var selectedBeneficiaries = model.Beneficiaries.Where(x => x.IsChecked).ToList();
                newPerson.Beneficiaries = selectedBeneficiaries.Select(x => new Beneficiary
                {
                    Id = x.Id
                }).ToList();
                foreach (var beneficiary in newPerson.Beneficiaries)
                {
                    RepositoryManager.Beneficiaries.Attach(beneficiary);
                }
            }

            var flats = new List<Flat> { flat };
            foreach (var getFlat in flats)
            {
                RepositoryManager.Flats.Attach(getFlat);
            }

            newPerson.Flats = flats;
            Add(newPerson);

            return true;
        }

        public IQueryable<PersonSelectionModel> SearchPersonsByFilter(FilterFileImportModel filter)
        {
            var personsIds = new List<int>();

            if(filter.DistrictIds != null)
            {
                var personIdsByDistricts = RepositoryManager.Districts
               .GetPersonsIdsByDistrictIds(filter.DistrictIds);
                personsIds.AddRange(personIdsByDistricts);
            }

            if (filter.CityIds != null)
            {
                var personIdsByCityIds = RepositoryManager.Cities
                    .GetPersonsIdsByCityIds(filter.CityIds?.ToList());
                personsIds.AddRange(personIdsByCityIds);
            }

            if (filter.StreetIds != null)
            {
                var personIdsByStreetIds = RepositoryManager.Streets
                    .GetPersonsIdsByStreetIds(filter.StreetIds?.ToList());
                personsIds.AddRange(personIdsByStreetIds);
            }

            if(filter.ApartmentIds != null)
            {
                var personIdsByApartmentIds = RepositoryManager.Apartments
                    .GetPersonsIdsByApartmentIds(filter.ApartmentIds?.ToList());
                personsIds.AddRange(personIdsByApartmentIds);
            }

            if(filter.FlatIds != null)
            {
                var personIdsByFlatIds = RepositoryManager.Flats
                    .GetPersonIdsByFlatIds(filter.FlatIds?.ToList());
                personsIds.AddRange(personIdsByFlatIds);
            }

            personsIds = personsIds.Distinct().ToList();
            var persons = RepositoryManager.Persons.GetPersonsByIds(personsIds).Where(x => x.IsDead == filter.IsDeadPerson);
            return persons;
        }

    }
}
