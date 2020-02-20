using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Svbase.Core.Repositories.Implementation
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public IQueryable<PersonSelectionModel> GetPersons()
        {
            var persons = DbSet
                .Select(x => new PersonSelectionModel()
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Position = x.Position,
                    Gender = x.Gender,
                    IsDead = x.IsDead,
                    Email = x.Email,
                    FirstMobilePhone = x.MobileTelephoneFirst,
                    SecondMobilePhone = x.MobileTelephoneSecond,
                    HomePhone = x.StationaryPhone,
                    PartionType = x.PartionType,
                    DateBirth = x.BirthdayDate,
                    Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).ToList(),
                    City = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.City.Id,
                        Name = f.Apartment.Street.City.Name,
                    }).FirstOrDefault(),
                    Street = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.Id,
                        Name = f.Apartment.Street.Name,
                    }).FirstOrDefault(),
                    Apartment = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Id,
                        Name = f.Apartment.Name,
                    }).FirstOrDefault(),
                    Flat = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Id,
                        Name = f.Number,
                    }).FirstOrDefault(),
                    Work = x.Work
                }).OrderBy(x => x.Id);
            return persons;
        }

        public async Task<List<PersonSelectionModel>> GetPersonIsNotDead()
        {
            var persons = await DbSet
                .Select(x => new PersonSelectionModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Position = x.Position,
                    Gender = x.Gender,
                    IsDead = x.IsDead,
                    Email = x.Email,
                    FirstMobilePhone = x.MobileTelephoneFirst,
                    SecondMobilePhone = x.MobileTelephoneSecond,
                    HomePhone = x.StationaryPhone,
                    PartionType = x.PartionType,
                    DateBirth = x.BirthdayDate,
                    Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).ToList(),
                    City = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.City.Id,
                        Name = f.Apartment.Street.City.Name,
                    }).FirstOrDefault(),
                    Street = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.Id,
                        Name = f.Apartment.Street.Name,
                    }).FirstOrDefault(),
                    Apartment = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Id,
                        Name = f.Apartment.Name,
                    }).FirstOrDefault(),
                    Flat = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Id,
                        Name = f.Number,
                    }).FirstOrDefault(),
                    Work = x.Work
                }).OrderBy(x => x.Id).Where(x => x.IsDead == false).ToListAsync();

            return persons;
        }

        public PersonViewModel GetPersonById(int id)
        {
            var person = DbSet.Select(x => new PersonViewModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                Email = x.Email,
                FirstMobilePhone = x.MobileTelephoneFirst,
                SecondMobilePhone = x.MobileTelephoneSecond,
                HomePhone = x.StationaryPhone,
                DateBirth = x.BirthdayDate,
                Work = x.Work,
                IsDead = x.IsDead,
                FlatId = x.Flats.FirstOrDefault().Id
            })
                .FirstOrDefault(x => x.Id == id);
            return person;
        }

        public PersonAndFullAddressViewModel GetPersonWithAddressById(int id)
        {
            var person = DbSet.Select(x => new PersonAndFullAddressViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.LastName,
                Email = x.Email,
                FirstMobilePhone = x.MobileTelephoneFirst,
                SecondMobilePhone = x.MobileTelephoneSecond,
                HomePhone = x.StationaryPhone,
                DateBirth = x.BirthdayDate,
                Gender = x.Gender,
                IsDead = x.IsDead,
                Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    IsChecked = true
                }).ToList(),
                Work = x.Work,
                FlatId = x.Flats.FirstOrDefault().Id,
                ApartmentId = x.Flats.FirstOrDefault().ApartmentId,
                StreetId = x.Flats.FirstOrDefault().Apartment.StreetId,
                CityId = x.Flats.FirstOrDefault().Apartment.Street.CityId
            })
                .FirstOrDefault(x => x.Id == id);
            return person;
        }

        public IQueryable<PersonSelectionModel> GetPersonsByIds(IEnumerable<int> ids)
        {
            if (ids == null) return null;
            var persons = DbSet
                .Where(x => ids.ToList().Contains(x.Id))
                .Select(x => new PersonSelectionModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    DateBirth = x.BirthdayDate,
                    Gender = x.Gender,
                    IsDead = x.IsDead,
                    Position = x.Position,
                    FirstMobilePhone = x.MobileTelephoneFirst,
                    SecondMobilePhone = x.MobileTelephoneSecond,
                    HomePhone = x.StationaryPhone,
                    Email = x.Email,
                    PartionType = x.PartionType,
                    Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).ToList(),
                    City = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.City.Id,
                        Name = f.Apartment.Street.City.Name,
                    }).FirstOrDefault(),
                    Street = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.Id,
                        Name = f.Apartment.Street.Name,
                    }).FirstOrDefault(),
                    Apartment = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Id,
                        Name = f.Apartment.Name,
                    }).FirstOrDefault(),
                    Flat = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Id,
                        Name = f.Number,
                    }).FirstOrDefault(),
                    Work = x.Work
                }).OrderBy(x => x.Id);
            return persons;
        }

        public IQueryable<PersonSelectionModel> SearchPersonsByFields(PersonSearchModel searchFields)
        {
            IQueryable<Person> searchTerms = DbSet;
            if (searchFields.IsFirstNameIncludedInSearch)
                searchTerms = searchTerms.Where(x => x.FirstName.Contains(searchFields.FirstName));
            if (searchFields.IsLastNameIncludedInSearch)
                searchTerms = searchTerms.Where(x => x.LastName.ToString().Contains(searchFields.LastName));
            if (searchFields.IsMiddleNameIncludedInSearch)
                searchTerms = searchTerms.Where(x => x.MiddleName.ToString().Contains(searchFields.MiddleName));
            if (searchFields.IsMobilePhoneIncludedInSearch)
                searchTerms =
                    searchTerms.Where(x => x.MobileTelephoneFirst.ToString().Contains(searchFields.MobilePhone)
                                           || x.MobileTelephoneSecond.ToString().Contains(searchFields.MobilePhone));

            var persons = searchTerms
                .Select(x => new PersonSelectionModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Position = x.Position,
                    Gender = x.Gender,
                    IsDead = x.IsDead,
                    Email = x.Email,
                    FirstMobilePhone = x.MobileTelephoneFirst,
                    SecondMobilePhone = x.MobileTelephoneSecond,
                    HomePhone = x.StationaryPhone,
                    PartionType = x.PartionType,
                    DateBirth = x.BirthdayDate,
                    Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).ToList(),
                    City = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.City.Id,
                        Name = f.Apartment.Street.City.Name,
                    }).FirstOrDefault(),
                    Street = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.Id,
                        Name = f.Apartment.Street.Name,
                    }).FirstOrDefault(),
                    Apartment = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Id,
                        Name = f.Apartment.Name,
                    }).FirstOrDefault(),
                    Flat = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Id,
                        Name = f.Number,
                    }).FirstOrDefault(),
                    Work = x.Work
                }).OrderBy(x => x.Id);
            return persons;
        }

        public async Task<List<PersonSelectionModel>> SearchPersonsByFieldsAsync(PersonSearchModel searchFields)
        {
           var list = new List<Person>();
            if (searchFields.IsFirstNameIncludedInSearch)
                list = await DbSet.Where(x => x.FirstName.Contains(searchFields.FirstName)).ToListAsync();
            if (searchFields.IsLastNameIncludedInSearch)
                list = await DbSet.Where(x => x.LastName.ToString().Contains(searchFields.LastName)).ToListAsync();
            if (searchFields.IsMiddleNameIncludedInSearch)
                list = await DbSet.Where(x => x.MiddleName.ToString().Contains(searchFields.MiddleName)).ToListAsync();
            if (searchFields.IsMobilePhoneIncludedInSearch)
                list = await DbSet.Where(x => x.MobileTelephoneFirst.ToString().Contains(searchFields.MobilePhone)
                                              || x.MobileTelephoneSecond.ToString().Contains(searchFields.MobilePhone)).ToListAsync();

            var persons = list
                .Select(x => new PersonSelectionModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Position = x.Position,
                    Gender = x.Gender,
                    IsDead = x.IsDead,
                    Email = x.Email,
                    FirstMobilePhone = x.MobileTelephoneFirst,
                    SecondMobilePhone = x.MobileTelephoneSecond,
                    HomePhone = x.StationaryPhone,
                    PartionType = x.PartionType,
                    DateBirth = x.BirthdayDate,
                    Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).ToList(),
                    City = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.City.Id,
                        Name = f.Apartment.Street.City.Name,
                    }).FirstOrDefault(),
                    Street = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.Id,
                        Name = f.Apartment.Street.Name,
                    }).FirstOrDefault(),
                    Apartment = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Id,
                        Name = f.Apartment.Name,
                    }).FirstOrDefault(),
                    Flat = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Id,
                        Name = f.Number,
                    }).FirstOrDefault(),
                    Work = x.Work
                }).OrderBy(x => x.Id).ToList();

            return persons;
        }

        public int GetAllPersonsCount()
        {
            return DbSet.Count();
        }

        public int GetAllPersonsWithMobilePhoneCount()
        {
            return DbSet.Count(x => x.MobileTelephoneFirst.Length > 0 || x.MobileTelephoneSecond.Length > 0);
        }

        public int GetPersonsWithoutBeneficiariesCount()
        {
            return DbSet.Count(x => !x.Beneficiaries.Any());
        }

        public int GetPersonsWidthMobilePhoneWithoutBeneficiariesCount()
        {
            return
                DbSet.Count(
                    x =>
                        !x.Beneficiaries.Any() &&
                        (x.MobileTelephoneFirst.Length > 0 || x.MobileTelephoneSecond.Length > 0));
        }

        public IQueryable<PersonSelectionModel> SearchDublicateByFirstAndLastName()
        {
            return DbSet.GroupBy(x => new { x.FirstName, x.LastName }).Where(x => x.Count() > 1).SelectMany(x => x)
                .Where(x => x.FirstName.Trim().Length > 0 && x.LastName.Trim().Length > 0)
                .Select(x => new PersonSelectionModel
                {
                    Id = x.Id,
                    FirstName = x.FirstName,
                    MiddleName = x.MiddleName,
                    LastName = x.LastName,
                    Position = x.Position,
                    Gender = x.Gender,
                    IsDead = x.IsDead,
                    Email = x.Email,
                    FirstMobilePhone = x.MobileTelephoneFirst,
                    SecondMobilePhone = x.MobileTelephoneSecond,
                    HomePhone = x.StationaryPhone,
                    PartionType = x.PartionType,
                    DateBirth = x.BirthdayDate,
                    Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                    {
                        Id = b.Id,
                        Name = b.Name
                    }).ToList(),
                    City = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.City.Id,
                        Name = f.Apartment.Street.City.Name,
                    }).FirstOrDefault(),
                    Street = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Street.Id,
                        Name = f.Apartment.Street.Name,
                    }).FirstOrDefault(),
                    Apartment = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Apartment.Id,
                        Name = f.Apartment.Name,
                    }).FirstOrDefault(),
                    Flat = x.Flats.Select(f => new BaseViewModel
                    {
                        Id = f.Id,
                        Name = f.Number,
                    }).FirstOrDefault(),
                    Work = x.Work
                }).OrderBy(x => x.LastName).ThenBy(x => x.FirstName);
        }


        public IQueryable<PersonSelectionModel> SearchDublicateByPhoneNumber()
        {
            return DbSet.GroupBy(x => new { x.MobileTelephoneFirst, x.MobileTelephoneSecond, x.StationaryPhone })
                .Where(x => x.Count() > 1)
                .SelectMany(x => x)
                .Where(
                    x =>
                        x.MobileTelephoneFirst.Trim().Length > 0 || x.MobileTelephoneSecond.Trim().Length > 0 ||
                        x.StationaryPhone.Trim().Length > 0).Select(x => new PersonSelectionModel
                        {
                            Id = x.Id,
                            FirstName = x.FirstName,
                            MiddleName = x.MiddleName,
                            LastName = x.LastName,
                            Position = x.Position,
                            Gender = x.Gender,
                            IsDead = x.IsDead,
                            Email = x.Email,
                            FirstMobilePhone = x.MobileTelephoneFirst,
                            SecondMobilePhone = x.MobileTelephoneSecond,
                            HomePhone = x.StationaryPhone,
                            PartionType = x.PartionType,
                            DateBirth = x.BirthdayDate,
                            Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                            {
                                Id = b.Id,
                                Name = b.Name
                            }).ToList(),
                            City = x.Flats.Select(f => new BaseViewModel
                            {
                                Id = f.Apartment.Street.City.Id,
                                Name = f.Apartment.Street.City.Name,
                            }).FirstOrDefault(),
                            Street = x.Flats.Select(f => new BaseViewModel
                            {
                                Id = f.Apartment.Street.Id,
                                Name = f.Apartment.Street.Name,
                            }).FirstOrDefault(),
                            Apartment = x.Flats.Select(f => new BaseViewModel
                            {
                                Id = f.Apartment.Id,
                                Name = f.Apartment.Name,
                            }).FirstOrDefault(),
                            Flat = x.Flats.Select(f => new BaseViewModel
                            {
                                Id = f.Id,
                                Name = f.Number,
                            }).FirstOrDefault(),
                            Work = x.Work
                        }).OrderBy(x => x.HomePhone).ThenBy(x => x.SecondMobilePhone).ThenBy(x => x.FirstMobilePhone);
        }

        public IEnumerable<PersonDublicateModel> GetPersonDublicateModel()
        {
            return DbSet.Select(x => new PersonDublicateModel
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                StreetName = x.Apartment.Street.Name,
                ApartmentNumber = x.Apartment.Name
            });
        }
    }
}
