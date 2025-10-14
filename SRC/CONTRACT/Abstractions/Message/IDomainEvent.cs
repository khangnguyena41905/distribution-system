using MediatR;

namespace COMMON.CONTRACT.Abstractions.Message;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}