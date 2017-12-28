using System.Collections.Generic;
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
    }
}
