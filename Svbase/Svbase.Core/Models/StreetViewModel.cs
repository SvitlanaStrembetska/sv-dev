using Svbase.Core.Data.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Svbase.Core.Models
{

    public class StreetBaseViewModel : BaseViewModel
    {
        [Display(Name = "Псевдонім")]
        public string Pseudonym { get; set; }
    }

    public class StreetCreateModel : StreetBaseViewModel
    {
        [Display(Name = "Псевдонім")]
       
        public bool CanDelete { get; set; }
        public int CityId { get; set; }
        public Street Update(Street street)
        {
            street.Id = Id;
            street.Name = Name;
            street.Pseudonym = Pseudonym;
            street.CityId = CityId;
            return street;
        }
    }

    public class StreetViewModel : StreetCreateModel
    {
        public IEnumerable<ApartmentCreateModel> Apartments { get; set; }
    }

    public class StreetSelectModel : StreetBaseViewModel
    {
        public bool IsChecked { get; set; }
        public string CityName { get; set; }
    }
}
