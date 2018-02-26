using System.Collections.Generic;

namespace Svbase.Core.Models
{
    public class ItemFilterModel
    {
        public string ParentName { get; set; }
        public List<BaseViewModel> Items { get; set; }
    }
}
