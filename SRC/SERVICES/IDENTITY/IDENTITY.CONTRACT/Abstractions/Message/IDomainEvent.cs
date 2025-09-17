using MediatR;

namespace IDENTITY.CONTRACT.Abstractions.Message;

public interface IDomainEvent : INotification
{
    public Guid Id { get; init; }
}