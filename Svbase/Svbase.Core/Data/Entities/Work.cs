using Svbase.Core.Data.Abstract;
using System.Collections.Generic;

namespace Svbase.Core.Data.Entities
{
    public class Work : Entity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Person> Persons { get; set; }

    }
}