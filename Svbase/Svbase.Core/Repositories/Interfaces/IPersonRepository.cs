using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        IEnumerable<PersonSelectionModel> GetPersons();
        PersonViewModel GetPersonById(int id);
        IEnumerable<PersonSelectionModel> GetPersonsByIds(IEnumerable<int> ids);
    }
}
