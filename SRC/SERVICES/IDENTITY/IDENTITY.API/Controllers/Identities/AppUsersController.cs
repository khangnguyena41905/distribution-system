using IDENTITY.API.Abstractions;
using IDENTITY.APPLICATION.Features.Identities.AppUsers;
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
        var result = await _sender.Send(new GetPagedAppUserQuery(pageIndex - 1, pageSize, search), cancellationToken);

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

}
