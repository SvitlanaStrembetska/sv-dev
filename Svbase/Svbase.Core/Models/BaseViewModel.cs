using System.ComponentModel.DataAnnotations;

namespace Svbase.Core.Models
{
    public class BaseViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Назва")]
        public string Name { get; set; }
    }
}
