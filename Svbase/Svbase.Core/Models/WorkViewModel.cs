using Svbase.Core.Data.Entities;

namespace Svbase.Core.Models
{
    public class WorkCreateModel : BaseViewModel
    {
        public bool CanDelete { get; set; }

        public Work Update(Work work)
        {
            work.Id = Id;
            work.Name = Name;
            return work;
        }
    }
}
