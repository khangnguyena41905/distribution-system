using INVENTORY.DOMAIN.Abstractions;
using MassTransit;
using MassTransit.Mediator;

namespace INVENTORY.INFRASTRUCTURE.EventHandlers;

public class MassTransitEventPublisher : IEventPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMediator _mediator;
    private readonly bool _useRabbitMq;

    public MassTransitEventPublisher(IPublishEndpoint publishEndpoint, IMediator mediator, bool useRabbitMq = true)
    {
        _publishEndpoint = publishEndpoint;
        _mediator = mediator;
        _useRabbitMq = useRabbitMq;
    }

    public async Task Publish<T>(T @event, CancellationToken cancellationToken = default)
        where T : class
    {
        if (_useRabbitMq)
            await _publishEndpoint.Publish(@event, cancellationToken);
        else
            await _mediator.Publish(@event, cancellationToken);
    }
}
