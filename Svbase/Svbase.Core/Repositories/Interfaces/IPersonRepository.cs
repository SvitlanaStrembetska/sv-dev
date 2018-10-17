using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        IQueryable<PersonSelectionModel> GetPersons();
        PersonViewModel GetPersonById(int id);
        PersonAndFullAddressViewModel GetPersonWithAddressById(int id);
        IQueryable<PersonSelectionModel> GetPersonsByIds(IEnumerable<int> ids);
        int GetAllPersonsCount();
    }
}
