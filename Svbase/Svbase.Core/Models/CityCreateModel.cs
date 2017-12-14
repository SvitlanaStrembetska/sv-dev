using Svbase.Core.Data.Entities;

namespace Svbase.Core.Models
{
    public class CityCreateModel : BaseViewModel
    {

        public City UpdateCity(City city)
        {
            city.Id = Id;
            city.Name = Name;
            return city;
        }

    }
}
