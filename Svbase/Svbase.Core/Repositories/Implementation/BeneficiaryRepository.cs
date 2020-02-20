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
    public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        public BeneficiaryRepository(ApplicationDbContext context)
            : base(context) { }

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
                PersonsCount = x.Persons.Count,
                PersonsWithMobilePhoneCount = x.Persons.Count(z => z.MobileTelephoneFirst.Length > 0 || z.MobileTelephoneSecond.Length > 0)
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

        public async Task<List<BeneficiaryCreateModel>> GetAllBeneficiariesAsync()
        {
            var beneficiaries = await DbSet.Select(x => new BeneficiaryCreateModel
            {
                Id = x.Id,
                Name = x.Name,
                CanDelete = !x.Persons.Any()
            }).ToListAsync();

            return beneficiaries;
        }

        public async Task<List<DashboardItemModel>> GetDashboardBeneficiariesAsync()
        {
            var beneficiaries = await DbSet.Select(x => new DashboardItemModel
            {
                Id = x.Id,
                Name = x.Name,
                PersonsCount = x.Persons.Count,
                PersonsWithMobilePhoneCount = x.Persons.Count(z => z.MobileTelephoneFirst.Length > 0 || z.MobileTelephoneSecond.Length > 0)
            }).ToListAsync();

            return beneficiaries;
        }

        public async Task<List<CheckboxItemModel>> GetBeneficiariesForSelectingAsync()
        {
            var beneficiaries = await DbSet.Select(x => new CheckboxItemModel
            {
                Id = x.Id,
                Name = x.Name,
                IsChecked = false
            }).ToListAsync();

            return beneficiaries;
        }
    }
}
