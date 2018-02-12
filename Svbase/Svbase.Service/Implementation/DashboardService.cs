using System.Collections.Generic;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class DashboardService : IDashboardService
    {
        protected IRepositoryManager RepositoryManager { get; }

        //don't remove unitOfWork 
        public DashboardService(IUnitOfWork unitOfWork, IRepositoryManager repositoryManager)
        {
            RepositoryManager = repositoryManager;
        }

        public DashboardViewModel GetDashboardViewModel()
        {
            var beneficiaries = RepositoryManager.Beneficiaries.GetDashboardBeneficiaries();
            var districtsModel = RepositoryManager.Districts.GetDashboardDistrictsModel();
            var dashboardViewModel = new DashboardViewModel
            {
                Beneficiaries = beneficiaries,
                DistrictsModel = districtsModel
            };
            return dashboardViewModel;
        }
    }
}
