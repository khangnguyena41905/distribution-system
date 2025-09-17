using IDENTITY.CONTRACT.Abstractions.Shared;
using MediatR;

namespace IDENTITY.CONTRACT.Abstractions.Message;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}