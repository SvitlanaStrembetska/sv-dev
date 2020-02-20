using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        IQueryable<PersonSelectionModel> GetPersons();
        Task<List<PersonSelectionModel>> GetPersonIsNotDead();
        PersonViewModel GetPersonById(int id);
        PersonAndFullAddressViewModel GetPersonWithAddressById(int id);
        IQueryable<PersonSelectionModel> GetPersonsByIds(IEnumerable<int> ids);
        IQueryable<PersonSelectionModel> SearchPersonsByFields(PersonSearchModel searchFields);
        Task<List<PersonSelectionModel>> SearchPersonsByFieldsAsync(PersonSearchModel searchFields);
        int GetAllPersonsCount();
        int GetAllPersonsWithMobilePhoneCount();
        int GetPersonsWithoutBeneficiariesCount();
        int GetPersonsWidthMobilePhoneWithoutBeneficiariesCount();

        IEnumerable<PersonDublicateModel> GetPersonDublicateModel();


        #region DIBLICATE SEARCH
        IQueryable<PersonSelectionModel> SearchDublicateByFirstAndLastName();
        IQueryable<PersonSelectionModel> SearchDublicateByPhoneNumber();

        #endregion
    }
}
