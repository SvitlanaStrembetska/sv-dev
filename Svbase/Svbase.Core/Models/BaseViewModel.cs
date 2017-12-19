using System.ComponentModel.DataAnnotations;

namespace Svbase.Core.Models
{
    public class BaseViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
