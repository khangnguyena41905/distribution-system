using IDENTITY.API.Abstractions;
using IDENTITY.APPLICATION.Features.Identities.Actions;
using IDENTITY.APPLICATION.Requests.Identities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IDENTITY.API.Controllers.Identities;

public class ActionsController : ApiBaseController
{
    public ActionsController(ISender sender) : base(sender)
    {
        
    }
    /// <summary>
    /// Lấy danh sách tất cả Actions
    /// GET: api/actions
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetActionsQuery());

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { errors = result.Error });
    }

    /// <summary>
    /// Lấy chi tiết Action theo Id
    /// GET: api/actions/{id}
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetActionByIdQuery(id), cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(new { errors = result.Error });
    }

    /// <summary>
    /// Tạo mới Action
    /// POST: api/actions
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateActionRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateActionCommand(
            request.Id,
            request.Name,
            request.SortOrder,
            request.IsActive
        );

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { errors = result.Error });
    }

    /// <summary>
    /// Cập nhật Action
    /// PUT: api/actions/{id}
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CreateOrUpdateActionRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateActionCommand(
            id,
            request.Name,
            request.SortOrder,
            request.IsActive
        );

        var result = await _sender.Send(command, cancellationToken);

        if (result.IsSuccess)
            return Ok(result.Value);

        return BadRequest(new { errors = result.Error });
    }

    /// <summary>
    /// Xoá Action
    /// DELETE: api/actions/{id}
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteActionCommand(id), cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return NotFound(new { errors = result.Error });
    }
    
}