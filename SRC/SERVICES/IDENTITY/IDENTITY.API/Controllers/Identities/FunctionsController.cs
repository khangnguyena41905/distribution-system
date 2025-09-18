using IDENTITY.API.Abstractions;
using IDENTITY.APPLICATION.Features.Identities.Functions;
using IDENTITY.APPLICATION.Requests.Identities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IDENTITY.API.Controllers.Identities;

public class FunctionsController : ApiBaseController
{
    public FunctionsController(ISender sender): base(sender)
    {
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetFunctionsQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { errors = result.Error });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetFunctionByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : NotFound(new { errors = result.Error });
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrUpdateFunctionRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateFunctionCommand(
            request.Name,
            request.Url,
            request.ParrentId,
            request.SortOrder,
            request.CssClass,
            request.IsActive
        );

        var result = await _sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { errors = result.Error });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CreateOrUpdateFunctionRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateFunctionCommand(
            id,
            request.Name,
            request.Url,
            request.ParrentId,
            request.SortOrder,
            request.CssClass,
            request.IsActive,
            request.ActionInFunctions
        );

        var result = await _sender.Send(command, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(new { errors = result.Error });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new DeleteFunctionCommand(id), cancellationToken);
        return result.IsSuccess ? NoContent() : NotFound(new { errors = result.Error });
    }
}