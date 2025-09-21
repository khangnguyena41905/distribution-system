using AutoMapper;
using IDENTITY.APPLICATION.Dtos.PermissionDtos;
using IDENTITY.CONTRACT.Abstractions.Message;
using IDENTITY.CONTRACT.Abstractions.Shared;
using IDENTITY.DOMAIN.Abstractions.Repositories.Identities;
using IDENTITY.DOMAIN.Entities.Identities;

namespace IDENTITY.APPLICATION.Features.Identities.Permissions;

public record GetUserPermissionFunctionsQuery(Guid UserId) : IQuery<List<FunctionDto>>;

internal class GetUserPermissionsQueryHandler : IQueryHandler<GetUserPermissionFunctionsQuery, List<FunctionDto>>
{
    private readonly IAppUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserPermissionsQueryHandler(IAppUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<List<FunctionDto>>> Handle(GetUserPermissionFunctionsQuery request, CancellationToken cancellationToken)
    {
        var (user, roles) = await _userRepository.GetUserWithRolesAsync(request.UserId);

        if (user == null)
            return Result.Failure<List<FunctionDto>>(Error.None);

        var permissions = roles
            .SelectMany(r => r.Permissions)
            .ToList();

        var functionIds = permissions
            .Select(p => p.FunctionId)
            .Distinct()
            .ToList();

        var actionMap = permissions
            .GroupBy(p => p.FunctionId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(p => p.ActionId).Distinct().ToList()
            );

        var functions = await _userRepository.GetFunctionsWithActionsAsync(functionIds);

        // Lọc các ActionInFunctions theo quyền của user
        foreach (var function in functions)
        {
            if (actionMap.TryGetValue(function.Id, out var allowedActionIds))
            {
                function.ActionInFunctions = function.ActionInFunctions
                    .Where(aif => allowedActionIds.Contains(aif.ActionId))
                    .ToList();
            }
            else
            {
                function.ActionInFunctions = new List<ActionInFunction>();
            }
        }

        var result = _mapper.Map<List<FunctionDto>>(functions.OrderBy(f => f.SortOrder).ToList());

        return Result.Success(result);
    }
}
