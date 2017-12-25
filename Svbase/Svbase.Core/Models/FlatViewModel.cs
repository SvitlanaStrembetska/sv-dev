using Svbase.Core.Data.Entities;
using System.Collections.Generic;

namespace Svbase.Core.Models
{
    public class FlatCreateModel : BaseViewModel
    {
        public int ApartmentId { get; set; }
        public bool CanDelete { get; set; }

        public Flat Update(Flat flat)
        {
            flat.Id = Id;
            flat.Number = Name;
            flat.ApartmentId = ApartmentId;
            return flat;
        }
    }

    public class FlatViewModel : FlatCreateModel
    {
        public IEnumerable<PersonListModel> Persons { get; set; }
    }

}
