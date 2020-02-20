﻿using Svbase.Core.Data.Entities;
using Svbase.Core.Models;
using Svbase.Core.Repositories.Abstract;
using System.Collections.Generic;

namespace Svbase.Core.Repositories.Interfaces
{
    public interface ICityRepository : IGenericRepository<City>
    {
        IEnumerable<CityCreateModel> GetCities();
        IEnumerable<CityViewModel> GetAllCities();
        CityViewModel GetCityById(int id);
        IEnumerable<BaseViewModel> GetStretsBaseModelByCityIds(IList<int> cityIds);
        IEnumerable<BaseViewModel> GetStreetsBaseModelByCityId(int id);
        IEnumerable<int> GetPersonsIdsByCityIds(List<int> cityIds);
        IEnumerable<DistrictPanelBodyItemModel> GetPanelBodyCities();
    }
}
