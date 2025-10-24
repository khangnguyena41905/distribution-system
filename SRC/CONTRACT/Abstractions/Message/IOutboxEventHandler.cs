using MassTransit;
namespace COMMON.CONTRACT.Abstractions.Message;

public interface IOutboxEventHandler<TOutboxEvent> : IConsumer<TOutboxEvent>
    where TOutboxEvent : class, IOutboxEvent
{
}