using System.Collections.Generic;
using Svbase.Core.Data.Entities;

namespace Svbase.Core.Models
{
    public class CityCreateModel : BaseViewModel
    {
        public bool CanDelete { get; set; }

        public City Update(City city)
        {
            city.Id = Id;
            city.Name = Name;
            return city;
        }
    }

    public class CityViewModel : CityCreateModel
    {
        public IEnumerable<StreetCreateModel> Streets { get; set; }
    }
}
