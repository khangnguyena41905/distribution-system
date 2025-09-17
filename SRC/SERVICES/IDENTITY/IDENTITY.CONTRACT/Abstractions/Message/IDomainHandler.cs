using MediatR;

namespace IDENTITY.CONTRACT.Abstractions.Message;

public interface IDomainHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}