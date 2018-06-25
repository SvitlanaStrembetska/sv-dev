using System.Collections.Generic;
using Svbase.Core.Data.Abstract;

namespace Svbase.Core.Data.Entities
{
    public class Work : Entity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Person> Persons { get; set; }

    }
}