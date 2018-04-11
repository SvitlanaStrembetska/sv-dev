using System.Collections.Generic;
using System.Linq;
using Svbase.Core.Data;
using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Implementation
{
    public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        public BeneficiaryRepository(ApplicationDbContext context)
            : base(context) {}

        public IEnumerable<BeneficiaryCreateModel> GetAllBeneficiaries()
        {
            var beneficiaries = DbSet.Select(x => new BeneficiaryCreateModel
            {
                Id = x.Id,
                Name = x.Name,
                CanDelete = !x.Persons.Any()
            });

            return beneficiaries;
        }

        public IEnumerable<DashboardItemModel> GetDashboardBeneficiaries()
        {
            var beneficiaries = DbSet.Select(x => new DashboardItemModel
            {
                Id = x.Id,
                Name = x.Name,
                PersonsCount = x.Persons.Count
            }).ToList();
            return beneficiaries;
        }

        public IEnumerable<CheckboxItemModel> GetBeneficiariesForSelecting()
        {
            var beneficiaries = DbSet.Select(x => new CheckboxItemModel
            {
                Id = x.Id,
                Name = x.Name,
                IsChecked = false
            });
            return beneficiaries;
        }

        public IEnumerable<PersonSelectionModel> GetPersonsByBeneficiariesId(int beneficiaryId)
        {
            var persons = DbSet
                .Where(x => x.Id == beneficiaryId)
                .Select(x => x.Persons)
                .FirstOrDefault();

            if (persons == null)
                return new List<PersonSelectionModel>();

            var personModels = persons.Select(x => new PersonSelectionModel()
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                DateBirth = x.BirthdayDate,
                Email = x.Email,
                FirstMobilePhone = x.MobileTelephoneFirst,
                SecondMobilePhone = x.MobileTelephoneSecond,
                HomePhone = x.StationaryPhone,
                PartionType = x.PartionType,
                Gender = x.Gender,
                Position = x.Position,
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
            }).ToList();
            return personModels;
        }
    }
}
