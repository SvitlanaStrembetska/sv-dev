using System.Collections.Generic;
using Svbase.Core.Data.Abstract;
using Svbase.Core.Enums;

namespace Svbase.Core.Data.Entities
{
    public class District : Entity<int>
    {
        public string Name { get; set; }
        public DistrictType DistrictType { get; set; }
        public virtual ICollection<Apartment> Apartments { get; set; }

    }
}