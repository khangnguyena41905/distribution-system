namespace COMMON.CONTRACT.Abstractions.Message;

public interface IOutboxEvent
{
    Guid Id { get; set; }
    DateTime CreatedDate{ get; set; }
}