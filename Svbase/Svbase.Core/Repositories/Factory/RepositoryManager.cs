using Svbase.Core.Data;
using Svbase.Core.Repositories.Implementation;
using Svbase.Core.Repositories.Interfaces;

namespace Svbase.Core.Repositories.Factory
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly ApplicationDbContext _context;

        #region Private Repository fields

        private IApartmentRepository _apartmentRepository;
        private IApplicationUserRepository _applicationUserRepository;
        private IBeneficiaryRepository _beneficiaryRepository;
        private ICityRepository _cityRepository;
        private IDistrictRepository _districtRepository;
        private IFlatRepository _flatRepository;
        private IPersonRepository _personRepository;
        private IStreetRepository _streetRepository;

        #endregion

        public RepositoryManager(ApplicationDbContext context)
        {
            _context = context;
        }

        public IApartmentRepository Apartments =>
            _apartmentRepository ?? (_apartmentRepository = new ApartmentRepository(_context));

        public IApplicationUserRepository ApplicationUsers =>
            _applicationUserRepository ?? (_applicationUserRepository = new ApplicationUserRepository(_context));

        public IBeneficiaryRepository Beneficiaries =>
            _beneficiaryRepository ?? (_beneficiaryRepository = new BeneficiaryRepository(_context));

        public ICityRepository Cities =>
            _cityRepository ?? (_cityRepository = new CityRepository(_context));

        public IDistrictRepository Districts =>
            _districtRepository ?? (_districtRepository = new DistrictRepository(_context));

        public IFlatRepository Flats =>
            _flatRepository ?? (_flatRepository = new FlatRepository(_context));

        public IPersonRepository Persons =>
            _personRepository ?? (_personRepository = new PersonRepository(_context));

        public IStreetRepository Streets =>
            _streetRepository ?? (_streetRepository = new StreetRepository(_context));
    }
}
