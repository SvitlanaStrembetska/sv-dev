using System.Collections.Generic;
using Svbase.Core.Data.Abstract;

namespace Svbase.Core.Data.Entities
{
    public class Street : Entity<int>
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }

        public virtual ICollection<Apartment> Apartments { get; set; }

        public virtual ICollection<District> Districts { get; set; }
    }
}