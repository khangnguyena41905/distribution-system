using COMMON.CONTRACT.Abstractions.Shared;
using MediatR;

namespace COMMON.CONTRACT.Abstractions.Message;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>{}