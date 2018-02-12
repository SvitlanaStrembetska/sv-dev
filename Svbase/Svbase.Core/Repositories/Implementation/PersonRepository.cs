using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(ApplicationDbContext context)
            : base(context) { }

        public IEnumerable<PersonViewModel> GetPersons()
        {
            var persons = DbSet.Select(x => new PersonViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                Position = x.Position,
                Gender = x.Gender,
                Email = x.Email,
                FirthtMobilePhone = x.MobileTelephoneFirst,
                SecondMobilePhone = x.MobileTelephoneSecond,
                HomePhone = x.StationaryPhone,
                PartionType = x.PartionType,
                DateBirth = x.BirthdayDate,
                Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList()
            });
            return persons;
        }

        public PersonViewModel GetPersonById(int id)
        {
            var person = DbSet.Select(x => new PersonViewModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    //MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    //Position = x.Position,
                    //Gender = x.Gender,
                    //Email = x.Email,
                    //FirthtMobilePhone = x.MobileTelephoneFirst,
                    //SecondMobilePhone = x.MobileTelephoneSecond,
                    //HomePhone = x.HomePhone,
                    //PartionType = x.PartionType,
                    //DateBirth = x.BirthdayDate
                //Streets = x.Streets.Select(s => new StreetCreateModel
                //{
                //    Id = s.Id,
                //    Name = s.Name,
                //    CanDelete = !s.Apartments.Any()
                //})
            })
                .FirstOrDefault(x => x.Id == id);
            return person;
        }

        public IEnumerable<PersonViewModel> GetPersonsByBeneficiariesId(int beneficiaryId)
        {
            var persons = DbSet.Select(x => x.Beneficiaries.Select(b => b.Id)).ToList()
                //    .Select(x => new PersonViewModel
                //    {
                //        Id = x.Id,
                //        FirstName = x.FirstName,
                //        LastName = x.LastName,
                //        MiddleName = x.MiddleName,
                //        DateBirth = x.BirthdayDate,
                //        Email = x.Email,
                //        FirthtMobilePhone = x.MobileTelephoneFirst,
                //        SecondMobilePhone = x.MobileTelephoneSecond,
                //        HomePhone = x.StationaryPhone,
                //        PartionType = x.PartionType,
                //        Gender = x.Gender,
                //        Position = x.Position
                //    });
                //return persons;
                ;
            return new List<PersonViewModel>();
        }
    }
}
