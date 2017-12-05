using Microsoft.AspNet.Identity.EntityFramework;
using Svbase.Core.Data.Abstract;

namespace Svbase.Core.Data.Entities
{
    public class ApplicationUser : IdentityUser, IEntity<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
