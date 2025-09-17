using IDENTITY.CONTRACT.Abstractions.Shared;
using MediatR;

namespace IDENTITY.CONTRACT.Abstractions.Message;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> 
    where TCommand : ICommand {}

public interface ICommandHandler<TCommand,TResponse> : IRequestHandler<TCommand, Result<TResponse>> 
    where TCommand : ICommand<TResponse>{}