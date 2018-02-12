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
            });
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

        public IEnumerable<PersonViewModel> GetPersonsByBeneficiariesId(int beneficiaryId)
        {
            var persons = DbSet
                .Where(x => x.Id == beneficiaryId)
                .Select(x => x.Persons)
                .FirstOrDefault();

            if (persons == null)
                return new List<PersonViewModel>();

            var personModels = persons.Select(x => new PersonViewModel
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                MiddleName = x.MiddleName,
                DateBirth = x.BirthdayDate,
                Email = x.Email,
                FirthtMobilePhone = x.MobileTelephoneFirst,
                SecondMobilePhone = x.MobileTelephoneSecond,
                HomePhone = x.StationaryPhone,
                PartionType = x.PartionType,
                Gender = x.Gender,
                Position = x.Position,
                Beneficiaries = x.Beneficiaries.Select(b => new CheckboxItemModel
                {
                    Id = b.Id,
                    Name = b.Name
                }).ToList()
            }).ToList();
            return personModels;
        }
    }
}
