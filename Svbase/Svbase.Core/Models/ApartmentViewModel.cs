using Svbase.Core.Data.Entities;
using System.Collections.Generic;

namespace Svbase.Core.Models
{
    public class ApartmentCreateModel : BaseViewModel
    {
        public int StreetId { get; set; }
        public bool CanDelete { get; set; }

        public Apartment Update(Apartment apartment)
        {
            apartment.Id = Id;
            apartment.Name = Name;
            apartment.StreetId = StreetId;
            return apartment;
        }
    }

    public class ApartmentViewModel : ApartmentCreateModel
    {
        public IEnumerable<FlatCreateModel> Flats { get; set; }
    }
}
