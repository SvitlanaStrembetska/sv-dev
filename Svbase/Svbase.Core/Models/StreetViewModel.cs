using Svbase.Core.Data.Entities;
using System.Collections.Generic;

namespace Svbase.Core.Models
{
    public class StreetCreateModel : BaseViewModel
    {
        public bool CanDelete { get; set; }
        public int CityId { get; set; }
        public Street Update(Street street)
        {
            street.Id = Id;
            street.Name = Name;
            street.CityId = CityId;
            return street;
        }
    }

    public class StreetViewModel : StreetCreateModel
    {
        public IEnumerable<ApartmentCreateModel> Apartments { get; set; }
    }
}
