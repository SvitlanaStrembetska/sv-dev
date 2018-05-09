using System.ComponentModel.DataAnnotations;

namespace Svbase.Core.Enums
{
    public enum DistrictType
    {
        [Display(Name = "Партійний")]
        Сonstituency = 1,
        [Display(Name = "Виборчий")]
        Custom = 2
    }
}
