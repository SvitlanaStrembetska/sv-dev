using System.Collections.Generic;

namespace Svbase.Core.Models
{
    public class ApartmentFilterModel
    {
        public BaseViewModel City { get; set; }
        public List<ItemFilterModel> Streets { get; set; }
    }
}
