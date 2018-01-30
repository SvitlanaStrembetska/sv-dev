using Svbase.Core.Data;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Service.Implementation;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Factory
{
    public class ServiceManager : IServiceManager
    {
        private IRepositoryManager _repositoryManager;
        private IUnitOfWork _unitOfWork;

        #region Private Services Fields

        private IApartmentService _apartmentService;
        private IApplicationUserService _applicationUserService;
        private IBeneficiaryService _beneficiaryService;
        private ICityService _cityService;
        private IDistrictService _districtService;
        private IFlatService _flatService;
        private IPersonService _personService;
        private IStreetService _streetService;
        private IDashboardService _dashboardService;

        #endregion

        public ServiceManager()
        {
            var dbContext = ApplicationDbContext.Create();
            Init(new UnitOfWork(dbContext), new RepositoryManager(dbContext));
        }

        public ServiceManager(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
        {
            Init(unitOfWork, repositoryManager);
        }

        private void Init(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
        {
            _unitOfWork = unitOfWork;
            _repositoryManager = repositoryManager;
        }

        #region Public Properties

        public IApartmentService ApartmentService =>
            _apartmentService ?? (_apartmentService = new ApartmentService(_unitOfWork, _repositoryManager));

        public IApplicationUserService ApplicationUserService =>
            _applicationUserService ??
            (_applicationUserService = new ApplicationUserService(_unitOfWork, _repositoryManager));

        public IBeneficiaryService BeneficiaryService =>
            _beneficiaryService ?? (_beneficiaryService = new BeneficiaryService(_unitOfWork, _repositoryManager));

        public ICityService CityService =>
            _cityService ?? (_cityService = new CityService(_unitOfWork, _repositoryManager));

        public IDistrictService DistrictService =>
            _districtService ?? (_districtService = new DistrictService(_unitOfWork, _repositoryManager));

        public IFlatService FlatService =>
            _flatService ?? (_flatService = new FlatService(_unitOfWork, _repositoryManager));

        public IPersonService PersonService =>
            _personService ?? (_personService = new PersonService(_unitOfWork, _repositoryManager));

        public IStreetService StreetService =>
            _streetService ?? (_streetService = new StreetService(_unitOfWork, _repositoryManager));

        public IDashboardService DashboardService =>
            _dashboardService ?? (_dashboardService = new DashboardService(_unitOfWork, _repositoryManager));

        #endregion
    }
}
