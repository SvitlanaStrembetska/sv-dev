﻿using Svbase.Core.Models;
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

        public DashboardManagementViewModel GetDashboardModel()
        {
            var beneficiaries = RepositoryManager.Beneficiaries.GetDashboardBeneficiaries();
            var districtsModel = RepositoryManager.Districts.GetAllDistricts();
            var cities = RepositoryManager.Cities.GetAllCities();
            var streats = RepositoryManager.Streets.GetStreetsForSelecting();
            var dashboardModel = new DashboardManagementViewModel
            {
                Beneficiaries = beneficiaries,
                DistrictsModel = districtsModel,
                CityViewModels = cities,
                StreetViewModels = streats,
            };
            return dashboardModel;
        }

        public DashboardViewModel GetDashboardViewModel()
        {
            var beneficiaries = RepositoryManager.Beneficiaries.GetDashboardBeneficiaries();
            var districtsModel = RepositoryManager.Districts.GetDashboardDistrictsModel();
            var allpersonsCount = RepositoryManager.Persons.GetAllPersonsCount();
            var personsCount = RepositoryManager.Persons.GetPersonsWithoutBeneficiariesCount();
            var personsWidthMobilePhoneCount = RepositoryManager.Persons.GetPersonsWidthMobilePhoneWithoutBeneficiariesCount();
            var dashboardViewModel = new DashboardViewModel
            {
                Beneficiaries = beneficiaries,
                DistrictsModel = districtsModel,
                AllPersonsCount = allpersonsCount,
                WithoutBeneficiaries = new DashboardPersonsWithoutBeneficiaries
                {
                    PersonsCount = personsCount,
                    PersonsWidthMobilePhoneCount = personsWidthMobilePhoneCount,
                }
            };
            return dashboardViewModel;
        }
    }
}
