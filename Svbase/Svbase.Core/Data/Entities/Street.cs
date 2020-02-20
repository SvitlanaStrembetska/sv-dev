using Svbase.Core.Data.Abstract;
using System.Collections.Generic;

namespace Svbase.Core.Data.Entities
{
    public class Street : Entity<int>
    {
        public string Name { get; set; }
        public string Pseudonym { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }

        public virtual ICollection<Apartment> Apartments { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}