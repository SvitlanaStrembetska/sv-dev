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

        //public bool CreateDistrictByModel(DistrictCreateModel model)
        //{
        //    if (model == null)
        //    {
        //        return false;
        //    }
        //    model.Streets = model.Streets.Where(x => x.IsChecked).ToList();
        //    var district = model.Update(new District());
        //    district.Streets = model.Streets.Select(x => new Street
        //    {
        //        Id = x.Id
        //    }).ToList();
        //    district.DistrictType = model.DistrictType;
        //    foreach (var street in district.Streets)
        //    {
        //        RepositoryManager.Streets.Attach(street);
        //    }
        //    Add(district);
        //    return true;
        //}

        //public DistrictCreateModel GetDistrictModelById(int id)
        //{
        //    var district = GetDistrictById(id);
        //    var streets = RepositoryManager.Streets.GetStreetsForSelecting().ToList();
        //    var selectedStreetIds = district.Streets.Select(x => x.Id).ToList();
        //    foreach (var street in streets)
        //    {
        //        if (selectedStreetIds.Contains(street.Id))
        //        {
        //            street.IsChecked = true;
        //        }
        //    }
        //    district.Streets = streets.ToList();
        //    return district;
        //}

        //public DistrictCreateModel GetDistrictById(int id)
        //{
        //    var district = RepositoryManager.Districts.GetDistrictById(id);
        //    return district;
        //}

        //public bool EditDistrictByModel(DistrictCreateModel model)
        //{
        //    if (model == null)
        //    {
        //        return false;
        //    }

        //    var district = FindById(model.Id);

        //    if (district == null)
        //    {
        //        return false;
        //    }
        //    district = model.Update(district);

        //    var allStreets = RepositoryManager.Streets.GetAll().ToList();
        //    model.Streets = model.Streets.Where(x => x.IsChecked).ToList();
        //    var selectedStreetIds = model.Streets.Select(x => x.Id);

        //    var removedStreets = district.Streets.Where(x => !selectedStreetIds.Contains(x.Id)).ToList();
        //    foreach (var street in removedStreets)
        //    {
        //        district.Streets.Remove(street);
        //    }

        //    var districtStreetIds = district.Streets.Select(x => x.Id);
        //    var addedStreets =
        //        allStreets.Where(x => selectedStreetIds.Contains(x.Id) && !districtStreetIds.Contains(x.Id));
        //    foreach (var street in addedStreets)
        //    {
        //        district.Streets.Add(street);
        //    }
        //    Update(district);
        //    return true;
        //}

        //public IEnumerable<BaseViewModel> GetStretsBaseModelByDistrictIds(IList<int> districtIds)
        //{
        //    if(districtIds == null) return new List<BaseViewModel>();
        //    var streets = RepositoryManager.Districts.GetStretsBaseModelByDistrictIds(districtIds);
        //    return streets;
        //}
    }
}
