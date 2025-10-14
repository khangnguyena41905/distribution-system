using COMMON.CONTRACT.Abstractions.Message;
using IDENTITY.DOMAIN.Abstractions.Entities;

namespace IDENTITY.DOMAIN.Abstractions.Aggregates;

public class AggregateRoot<T> : DomainEntity<T>
{
    private readonly List<IDomainEvent> _domainEvents = new List<IDomainEvent>();
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);    
}