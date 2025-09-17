using IDENTITY.CONTRACT.Abstractions.Shared;
using MediatR;

namespace IDENTITY.CONTRACT.Abstractions.Message;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}