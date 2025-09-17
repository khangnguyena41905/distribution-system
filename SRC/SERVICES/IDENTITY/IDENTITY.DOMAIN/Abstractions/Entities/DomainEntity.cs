namespace IDENTITY.DOMAIN.Abstractions.Entities;

public abstract class DomainEntity<TKey>
{
    public virtual TKey Id { get; set; }
    public bool IsTransient()
    {
        return Id.Equals(default(TKey));
    }
}