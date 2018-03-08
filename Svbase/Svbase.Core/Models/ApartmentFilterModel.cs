using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Svbase.Core.Models
{
    public class ApartmentFilterModel
    {
        public BaseViewModel City { get; set; }
        public List<ItemFilterModel> Streets { get; set; }
    }   
}
