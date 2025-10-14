using COMMON.CONTRACT.Abstractions.Message;
using INVENTORY.DOMAIN.Abstractions.Entities;

namespace INVENTORY.DOMAIN.Abstractions.Aggregates;

public class AggregateRoot<Tkey> : DomainEntity<Tkey>
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);    
}