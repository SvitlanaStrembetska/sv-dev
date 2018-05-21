﻿using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Enums;
using Svbase.Core.Models;
using Svbase.Service.Abstract;

namespace Svbase.Service.Interfaces
{
    public interface IDistrictService : IEntityService<District>
    {
        IEnumerable<DistrictListModel> GetAllDistricts();

        bool CreateDistrictBy(DistrictListModel model);

        IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyDistrictsBy(DistrictType districtType);
        IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyCitiesBy(int districtId);
        DistrictViewInitDataModel GetDistrictViewInitDataModel(DistrictType districtType);

        IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyStreetsBy(int cityId);

        IEnumerable<DistrictPanelBodyApartmentModel> GetPanelBodyApartmentsBy(DistrictViewApartmentSearchFilter filter);
        //bool CreateDistrictByModel(DistrictCreateModel model);
        //DistrictCreateModel GetDistrictModelById(int id);
        //DistrictCreateModel GetDistrictById(int id);
        //bool EditDistrictByModel(DistrictCreateModel model);

        //IEnumerable<BaseViewModel> GetStretsBaseModelByDistrictIds(IList<int> districtIds);
    }
}
