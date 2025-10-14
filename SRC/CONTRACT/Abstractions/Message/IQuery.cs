using COMMON.CONTRACT.Abstractions.Shared;
using MediatR;

namespace COMMON.CONTRACT.Abstractions.Message;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}