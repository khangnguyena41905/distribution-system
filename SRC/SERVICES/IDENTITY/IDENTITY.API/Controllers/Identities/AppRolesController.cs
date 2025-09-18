using IDENTITY.API.Abstractions;
using IDENTITY.APPLICATION.Features.Identities.AppRoles;
using Microsoft.AspNetCore.Mvc;
using MediatR;

namespace IDENTITY.API.Controllers.Identities;

public class AppRolesController : ApiBaseController
{
    public AppRolesController(ISender sender) : base(sender)
    {
        
    }

    /// <summary>
    /// Lấy danh sách AppRole có phân trang
    /// GET: api/approles?pageIndex=1&pageSize=10
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int pageIndex = 1, [FromQuery] int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetAllAppRoleQuery(pageIndex, pageSize), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { errors = result.Error });
    }

    /// <summary>
    /// Lấy chi tiết AppRole theo Id
    /// GET: api/approles/{id}
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetAppRoleByIdQuery(id), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(new { errors = result.Error });
    }

    /// <summary>
    /// Tạo mới AppRole
    /// POST: api/approles
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppRoleCommand request, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { errors = result.Error });
    }

    /// <summary>
    /// Cập nhật AppRole
    /// PUT: api/approles/{id}
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAppRoleCommand request, CancellationToken cancellationToken)
    {

        var result = await _sender.Send(request, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { errors = result.Error });
    }

    /// <summary>
    /// Xoá AppRole
    /// DELETE: api/approles/{id}
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteAppRoleCommand(id), cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return NotFound(new { errors = result.Error });
    }
}
