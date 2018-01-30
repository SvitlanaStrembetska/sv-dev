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
            :base(unitOfWork,repositoryManager,repositoryManager.Persons)
        {
            
        }

        public IEnumerable<PersonViewModel> GetPersons()
        {
            var persons = RepositoryManager.Persons.GetPersons();
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

        public IEnumerable<BaseViewModel> GetFlatsBaseModelByApatrmentIds(IList<int> apartmentIds)
        {
            var flats = RepositoryManager.Apartments.GetFlatBaseModelByApartmentIds(apartmentIds);
            return flats;
        }
    }
}
