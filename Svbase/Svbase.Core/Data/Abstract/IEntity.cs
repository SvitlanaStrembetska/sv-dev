namespace Svbase.Core.Data.Abstract
{
    public interface IEntity
    {
    }

    public interface IEntity<T> : IEntity
    {
        T Id { get; set; }
    }
}
