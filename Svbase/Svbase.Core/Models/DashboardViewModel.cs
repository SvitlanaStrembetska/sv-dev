using System.Collections.Generic;
using Svbase.Core.Enums;

namespace Svbase.Core.Models
{
    public class DashboardViewModel
    {
        public IEnumerable<DashboardItemModel> Beneficiaries { get; set; }
        public DashboardDistrictsModel DistrictsModel { get; set; }
    }

    public class DashboardDistrictsModel
    {
        public IEnumerable<DashboardItemModel> ConstituencyDistrcits { get; set; }
        public IEnumerable<DashboardItemModel> CustomDistricts { get; set; }
    }

    public class DashboardItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PersonsCount { get; set; }
    }

    public class DashboardDistrictItemModel : DashboardItemModel
    {
        public DistrictType DistrictType { get; set; }
    }
}
