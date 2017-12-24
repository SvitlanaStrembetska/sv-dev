using System.Collections.Generic;
using Svbase.Core.Data.Entities;
using Svbase.Core.Enums;

namespace Svbase.Core.Models
{
    public class FilterSearchModel
    {
        public ICollection<int> StreatIds { get; set; }
        public ICollection<int> DistrictIds { get; set; }
        public DistrictType DistrictType { get; set; }
        public ICollection<int> FlatIds { get; set; }
        public ICollection<int> ApartmentIds { get; set; }
        public ICollection<int> BeneficiaryIds { get; set; }
    }
}
