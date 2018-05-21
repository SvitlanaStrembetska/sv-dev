using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Enums;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using Svbase.Core.Repositories.Factory;
using Svbase.Core.Repositories.Interfaces;
using Svbase.Service.Abstract;
using Svbase.Service.Interfaces;

namespace Svbase.Service.Implementation
{
    public class DistrictService : EntityService<IDistrictRepository, District>, IDistrictService
    {
        public DistrictService(IUnitOfWork unitOfWork,IRepositoryManager repositoryManager)
            :base(unitOfWork,repositoryManager,repositoryManager.Districts)
        {
            
        }

        public IEnumerable<DistrictListModel> GetAllDistricts()
        {
            return RepositoryManager.Districts.GetAllDistricts();
        }

        public bool CreateDistrictBy(DistrictListModel model)
        {
            if (string.IsNullOrEmpty(model?.Name))
                return false;
            if (model.DistrictType == 0)
                return false;

            var district = model.Update(new District());
            Add(district);
            return true;
        }

        public IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyDistrictsBy(DistrictType districtType)
        {
            var districts = RepositoryManager.Districts.GetPanelBodyDistrictsBy(districtType);
            return districts;
        }

        public IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyCitiesBy(int districtId)
        {
            var cities = RepositoryManager.Districts.GetPanelBodyCitiesBy(districtId);
            return cities;
        }

        public DistrictViewInitDataModel GetDistrictViewInitDataModel(DistrictType districtType)
        {
            var districts = GetPanelBodyDistrictsBy(districtType);
            var cities = RepositoryManager.Cities.GetPanelBodyCities();
            var model = new DistrictViewInitDataModel
            {
                Districts = districts,
                Cities = cities
            };
            return model;
        }

        public IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyStreetsBy(int cityId)
        {
            var streets = RepositoryManager.Streets.GetPanelBodyStreetsBy(cityId);
            return streets;
        }

        public IEnumerable<DistrictPanelBodyApartmentModel> GetPanelBodyApartmentsBy(DistrictViewApartmentSearchFilter filter)
        {
            var apartments = RepositoryManager.Apartments.GetPanelBodyApartmentsBy(filter);
            return apartments;
        }
    }
}
