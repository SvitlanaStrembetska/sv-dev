using Svbase.Core.Data.Abstract;
using System.Collections.Generic;

namespace Svbase.Core.Data.Entities
{
    public class Beneficiary : Entity<int>
    {
        public string Name { get; set; }

        public virtual ICollection<Person> Persons { get; set; }

    }
}