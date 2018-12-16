using System.Collections.Generic;
using Svbase.Core.Enums;

namespace Svbase.Core.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<DashboardItemModel> Beneficiaries { get; set; }
        public DashboardPersonsBeneficiaries WithoutBeneficiaries { get; set; }
        public DashboardDistrictsModel DistrictsModel { get; set; }
        public DashboardPersonsBeneficiaries AllPersons { get; set; }
    }

    public class DashboardManagementViewModel
    {
        public IEnumerable<DistrictListModel> DistrictsModel { get; set; }
        public IEnumerable<CityViewModel> CityViewModels { get; set; }
        public IEnumerable<StreetSelectModel> StreetViewModels { get; set; }
        public IEnumerable<ApartmentViewModel> Apartments { get; set; }
        public IEnumerable<FlatViewModel> FlatViewModels { get; set; }
        public IEnumerable<DashboardItemModel> Beneficiaries { get; set; }
    }

    public class DashboardDistrictsModel
    {
        public IEnumerable<DashboardItemModel> ConstituencyDistrcits { get; set; }
        public IEnumerable<DashboardItemModel> CustomDistricts { get; set; }
    }

    public class DashboardPersonsBeneficiaries
    {
        public int PersonsCount { get; set; }
        public int PersonsWidthMobilePhoneCount { get; set; }
    }

    public class DashboardItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PersonsCount { get; set; }
        public int PersonsWithMobilePhoneCount { get; set; }
    }

    public class DashboardDistrictItemModel : DashboardItemModel
    {
        public DistrictType DistrictType { get; set; }
    }
}
