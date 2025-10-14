using MediatR;

namespace COMMON.CONTRACT.Abstractions.Message;

public interface IDomainHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}