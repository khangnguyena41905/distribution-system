using IDENTITY.CONTRACT.Abstractions.Shared;
using MediatR;

namespace IDENTITY.CONTRACT.Abstractions.Message;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>{}