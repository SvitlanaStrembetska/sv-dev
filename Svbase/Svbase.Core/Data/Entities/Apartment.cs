using System.Collections.Generic;
using Svbase.Core.Data.Abstract;

namespace Svbase.Core.Data.Entities
{
    public class Apartment : Entity<int>
    {
        public string Name { get; set; }
        public int StreetId { get; set; }
        public virtual Street Street { get; set; }
        public virtual ICollection<Flat> Flats { get; set; }
        public virtual ICollection<District> Districts { get; set; }
    }
}
