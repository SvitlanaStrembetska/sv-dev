using Svbase.Core.Data.Abstract;
using Svbase.Core.Enums;
using System.Collections.Generic;

namespace Svbase.Core.Data.Entities
{
    public class District : Entity<int>
    {
        public string Name { get; set; }
        public DistrictType DistrictType { get; set; }
        public virtual ICollection<Apartment> Apartments { get; set; }

    }
}