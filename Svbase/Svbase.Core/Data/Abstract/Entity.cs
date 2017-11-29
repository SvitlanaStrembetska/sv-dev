using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Svbase.Core.Data.Abstract
{
    public abstract class BaseEntity : IEntity
    {
    }

    public abstract class Entity<T> : BaseEntity, IEntity<T>
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual T Id { get; set; }
    }
}
