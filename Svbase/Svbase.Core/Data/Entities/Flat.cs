using System.Collections.Generic;
using Svbase.Core.Data.Abstract;

namespace Svbase.Core.Data.Entities
{
    public class Flat : Entity<int>
    {
        public string Number { get; set; }
        public int ApartmentId { get; set; }
        public Apartment Apartment { get; set; }
        public virtual ICollection<Person> Persons { get; set; }

    }
}