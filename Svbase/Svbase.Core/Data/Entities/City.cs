using Svbase.Core.Data.Abstract;
using System.Collections.Generic;

namespace Svbase.Core.Data.Entities
{
    public class City : Entity<int>
    {
        public string Name { get; set; }
        public virtual ICollection<Street> Streets { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
    }
}