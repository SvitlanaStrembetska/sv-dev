using System.Collections.Generic;

namespace Svbase.Core.Models
{
    public class ItemFilterModel
    {
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public List<StreetBaseViewModel> Items { get; set; }
    }
}
