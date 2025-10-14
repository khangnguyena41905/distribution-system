using COMMON.CONTRACT.Abstractions.Message;
using COMMON.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.Functions;

public record DeleteFunctionCommand(string Id) : ICommand<bool>;

internal class DeleteFunctionCommandHandler : ICommandHandler<DeleteFunctionCommand, bool>
{
    private readonly IFunctionRepository _functionRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteFunctionCommandHandler(IFunctionRepository functionRepository, IUnitOfWork unitOfWork)
    {
        _functionRepository = functionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteFunctionCommand request, CancellationToken cancellationToken)
    {
        var func = await _functionRepository.FindByIdAsync(request.Id);
        if (func is null)
            return Result.Failure<bool>(new Error("404", "Function not found"));

        await _functionRepository.RemoveAsync(func);
        await _unitOfWork.CommitAsync();

        return Result.Success(true);
    }
}
