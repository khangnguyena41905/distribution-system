using IDENTITY.API.Abstractions;
using IDENTITY.APPLICATION.Features.Identities.AppUsers;
using IDENTITY.APPLICATION.Features.Identities.Permissions;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace IDENTITY.API.Controllers.Identities;

[Route("api/[controller]")]
[ApiController]
public class AppUsersController : ApiBaseController
{
    public AppUsersController(ISender sender) : base(sender)
    {
    }

    /// <summary>
    /// Lấy danh sách AppUser có phân trang
    /// GET: api/appusers?pageIndex=1&pageSize=10&search=nam
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetPagedAppUserQuery(pageIndex, pageSize, search), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { errors = result.Error });
    }

    /// <summary>
    /// Lấy chi tiết AppUser theo Id
    /// GET: api/appusers/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAppUserByIdQuery(id), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(new { errors = result.Error });
    }

    /// <summary>
    /// Tạo mới AppUser
    /// POST: api/appusers
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppUserCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { errors = result.Error });
    }

    /// <summary>
    /// Cập nhật AppUser
    /// PUT: api/appusers/{id}
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppUserCommand request, CancellationToken cancellationToken)
    {
        if (id != request.Id)
            return BadRequest(new { errors = "Id trong route và body không khớp." });

        var result = await _sender.Send(request, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { errors = result.Error });
    }

    // (Optional) DELETE nếu có logic xóa AppUser

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteAppUserCommand(id), cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return NotFound(new { errors = result.Error });
    }

    // // GET api/appusers/{userId}/roles
    // [HttpGet("{userId:guid}/roles")]
    // public async Task<IActionResult> GetUserWithRoles(Guid userId, CancellationToken cancellationToken)
    // {
    //     var result = await _sender.Send(new GetUserWithRolesQuery(userId), cancellationToken);
    //     if (result.IsSuccess)
    //         return Ok(new 
    //         {
    //             User = result.Value.User,
    //             Roles = result.Value.Roles
    //         });
    //
    //     return NotFound(new { errors = result.Error });
    // }

    // POST api/appusers/{userId}/roles
    [HttpPost("{userId:guid}/roles")]
    public async Task<IActionResult> AssignRole(Guid userId, List<Guid> roleId, CancellationToken cancellationToken)
    {
        var command = new AssignRolesToUserCommand(userId, roleId);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok();

        return BadRequest(new { errors = result.Error });
    }

    // GET api/appusers/{userId}/permissions/check?functionId=xxx&actionId=yyy
    [HttpGet("{userId:guid}/permissions/check")]
    public async Task<IActionResult> CheckPermission(Guid userId, [FromQuery] string functionId, [FromQuery] string actionId, CancellationToken cancellationToken)
    {
        var query = new CheckUserPermissionQuery(userId, functionId, actionId);
        var result = await _sender.Send(query, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { errors = result.Error });
    }
    // POST api/roles/{roleId}/permissions
    [HttpPost("{roleId:guid}/permissions")]
    public async Task<IActionResult> AssignPermissionsToRole(Guid roleId, [FromBody] List<PermissionDto> permissions, CancellationToken cancellationToken)
    {
        var command = new AssignPermissionsToRoleCommand(
            roleId,
            permissions.Select(p => (p.FunctionId, p.ActionId)).ToList()
        );

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok();

        return BadRequest(new { errors = result.Error });
    }

    public class PermissionDto
    {
        public string FunctionId { get; set; }
        public string ActionId { get; set; }
    }
}
