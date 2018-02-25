using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface IPersonRepository : IGenericRepository<Person>
    {
        IEnumerable<PersonViewModel> GetPersons();
        PersonViewModel GetPersonById(int id);
        IEnumerable<PersonViewModel> GetPersonsByBeneficiariesId(int beneficiaryId);
        IEnumerable<PersonViewModel> GetPersonsByIds(IEnumerable<int> ids);
    }
}
