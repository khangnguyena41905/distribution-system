using COMMON.CONTRACT.Abstractions.Message;

namespace INVENTORY.DOMAIN.Abstractions.Entities;

public abstract class DomainEntity<TKey>
{
    public virtual TKey Id { get; init; }
    public bool IsTransient()
    {
        return Id.Equals(default(TKey));
    }
}