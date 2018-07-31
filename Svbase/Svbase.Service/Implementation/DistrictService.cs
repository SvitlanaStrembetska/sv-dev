using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<BaseViewModel> GetDistrictsByType(DistrictType districtType)
        {
            return RepositoryManager.Districts.GetDistrictsByType(districtType);
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

        public bool SaveDistrictBy(SaveDistrictModel model)
        {
            if (!IsValid(model))
                return false;
            var district = FindById(model.DistrictId);
            if (district == null)
                return false;
            if (model.ApartmentIds == null)
            {
                model.ApartmentIds = new List<int>();
            }
            var oldStreetApartmentIds = district.Apartments
                .Where(x => x.StreetId == model.StreetId)
                .Select(x => x.Id)
                .ToList();

            var needToRemoveApartmentIds = oldStreetApartmentIds
                .Where(apartmentId => !model.ApartmentIds.Contains(apartmentId))
                .ToList();

            var needToAddApartmentIds = model.ApartmentIds
                .Where(apartmentId => !oldStreetApartmentIds.Contains(apartmentId))
                .ToList();
            var needToAddApartments = RepositoryManager.Apartments.GetByIds(needToAddApartmentIds).ToList();

            foreach (var needToRemoveApartmentId in needToRemoveApartmentIds)
            {
                var apartment = district.Apartments.FirstOrDefault(x => x.Id == needToRemoveApartmentId);
                district.Apartments.Remove(apartment);
            }

            foreach (var needToAddApartment in needToAddApartments)
            {
                district.Apartments.Add(needToAddApartment);
            }

            Update(district);
            return true;
        }

        private bool IsValid(SaveDistrictModel model)
        {
            if (model == null)
                return false;
            if (model.StreetId == 0)
                return false;
            return true;
        }
    }
}
