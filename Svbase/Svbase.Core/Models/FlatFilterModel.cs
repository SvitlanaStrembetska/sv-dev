using System.Collections.Generic;

namespace Svbase.Core.Models
{
    public class CityFlatFilterModel
    {
        public BaseViewModel City { get; set; }
        public List<StreetFlatFilterModel> Streets { get; set; }
    }

    public class StreetFlatFilterModel
    {
        public BaseViewModel Street { get; set; }
        public List<ApartmentFlatModel> Apartments { get; set; }
    }

    public class ApartmentFlatModel
    {
        public BaseViewModel Apartment { get; set; }
        public List<BaseViewModel> Flats { get; set; }
    }

}
