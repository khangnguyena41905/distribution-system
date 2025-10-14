using COMMON.CONTRACT.Abstractions.Shared;
using MediatR;

namespace COMMON.CONTRACT.Abstractions.Message;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}