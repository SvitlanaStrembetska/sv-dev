using System.Collections.Generic;

namespace Svbase.Core.Models
{
    public class StreetSearchFilterModel
    {
        public IList<int> CityIds { get; set; }
        public IList<int> DistrictIds { get; set; }
    }
}
