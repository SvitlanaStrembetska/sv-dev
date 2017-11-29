using System.Collections.Generic;
using Svbase.Core.Data.Abstract;

namespace Svbase.Core.Data.Entities
{
    public class City : Entity<int>
    {
        public string Name { get; set; }
        public virtual ICollection<Street> Streets { get; set; }

    }
}