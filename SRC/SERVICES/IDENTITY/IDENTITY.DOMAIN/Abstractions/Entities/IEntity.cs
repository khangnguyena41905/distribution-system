namespace IDENTITY.DOMAIN.Abstractions.Entities;

public interface IEntity<T>
{   
}

public class Entity<T>:IEntity<T>
{
    public T Id { get; protected set; }
    public bool IsDeleted { get; protected set; }
}