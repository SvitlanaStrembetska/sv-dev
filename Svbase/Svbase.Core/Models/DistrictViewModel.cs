using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Enums;

namespace Svbase.Core.Models
{
    public class DistrictListModel : BaseViewModel
    {
        public DistrictType DistrictType { get; set; }
    }

    public class DistrictCreateModel : DistrictListModel
    {
        public IList<StreetSelectModel> Streets { get; set; }

        public District Update(District district)
        {
            district.Id = Id;
            district.Name = Name;
            district.DistrictType = DistrictType;
           
            return district;
        }
    }
}
