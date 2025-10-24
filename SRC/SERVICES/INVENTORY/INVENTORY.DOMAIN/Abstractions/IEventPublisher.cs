namespace INVENTORY.DOMAIN.Abstractions;

public interface IEventPublisher
{
    Task Publish<T>(T @event, CancellationToken cancellationToken = default)
        where T : class;
}
