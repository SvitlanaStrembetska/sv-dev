using Svbase.Core.Data.Entities;
using Svbase.Core.Enums;
using System.Collections.Generic;

namespace Svbase.Core.Models
{
    public class DistrictListModel : BaseViewModel
    {
        public DistrictType DistrictType { get; set; }

        public District Update(District district)
        {
            district.Name = Name;
            district.DistrictType = DistrictType;
            return district;
        }
    }

    public class DistrictPanelBodyItemModel : BaseViewModel
    {
        public DistrictPanelBodyItemType DistrictPanelBodyItemType { get; set; }
    }

    public class DistrictCreateModel : DistrictListModel
    {
        public IList<StreetSelectModel> Streets { get; set; }
    }

    public class DistrictViewInitDataModel
    {
        public IEnumerable<DistrictPanelBodyItemModel> Cities { get; set; }
        public IEnumerable<DistrictPanelBodyItemModel> Districts { get; set; }
    }

    public class DistrictViewApartmentSearchFilter
    {
        public int DistrictId { get; set; }
        public int StreetId { get; set; }
    }

    public class DistrictPanelBodyApartmentModel : DistrictPanelBodyItemModel
    {
        public bool IsChecked { get; set; }
    }

    public class SaveDistrictModel
    {
        public int DistrictId { get; set; }
        public int StreetId { get; set; }
        public IEnumerable<int> ApartmentIds { get; set; }
    }

}
