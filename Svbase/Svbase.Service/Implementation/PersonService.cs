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

        public IEnumerable<PersonViewModel> GetPersons()
        {
            var persons = RepositoryManager.Persons.GetPersons();
            return persons;
        }
        public IEnumerable<PersonViewModel> GetPersonsByBeneficiariesId(int beneficiaryId)
        {
            var persons = RepositoryManager.Beneficiaries.GetPersonsByBeneficiariesId(beneficiaryId);
            return persons;
        }

        public PersonViewModel GetPersonById(int id)
        {
            var person = RepositoryManager.Persons.GetPersonById(id);
            return person;
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

        public IEnumerable<BaseViewModel> GetFlatsBaseModelByApatrmentIds(IList<int> apartmentIds)
        {
            var flats = RepositoryManager.Apartments.GetFlatBaseModelByApartmentIds(apartmentIds);
            return flats;
        }

        public bool CreatePersonByModel(PersonViewModel model)
        {
            if (model == null)
            {
                return false;
            }

            var newPerson = model.Update(new Person());
            var selectedBeneficiaries = model.Beneficiaries
                .Where(x => x.IsChecked)
                .ToList();
            newPerson.Beneficiaries = selectedBeneficiaries
                .Select(x => new Beneficiary
                {
                    Id = x.Id
                }).ToList();
            foreach (var beneficiary in newPerson.Beneficiaries)
            {
                RepositoryManager.Beneficiaries.Attach(beneficiary);
            }
            var flats = new List<Flat>
            {
                new Flat
                {
                    Id = model.FlatId
                }
            };
            foreach (var flat in flats)
            {
                RepositoryManager.Flats.Attach(flat);
            }
            newPerson.Flats = flats;
            Add(newPerson);
            return true;
        }

        public List<PersonViewModel> SearchPersonsByFilter(FilterSearchModel filter)
        {
            var personsIds = new List<int>();

            var personIdsByDistricts = RepositoryManager.Districts
                .GetPersonsIdsByDistrictIds(filter.DistrictIds);

            if (filter.StreetIds == null && filter.CityIds != null)
            {
                var personIdsByCityIds = RepositoryManager.Cities
                    .GetPersonsIdsByCityIds(filter.CityIds?.ToList());
                personsIds.AddRange(personIdsByCityIds);
            }

            if (filter.ApartmentIds == null && filter.StreetIds != null)
            {
                var personIdsByStreetIds = RepositoryManager.Streets
                    .GetPersonsIdsByStreetIds(filter.StreetIds?.ToList());
                personsIds.AddRange(personIdsByStreetIds);
            }
            personsIds.AddRange(personIdsByDistricts);

            personsIds = personsIds.Distinct().ToList();
            var persons = RepositoryManager.Persons.GetPersonsByIds(personsIds);
            return persons.ToList();
        }
    }
}
