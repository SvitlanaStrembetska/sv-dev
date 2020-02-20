using Svbase.Core.Data.Abstract;
using System.Collections.Generic;

namespace Svbase.Core.Data.Entities
{
    public class Flat : Entity<int>
    {
        public string Number { get; set; }
        public int ApartmentId { get; set; }
        public virtual Apartment Apartment { get; set; }
        public virtual ICollection<Person> Persons { get; set; }

    }
}