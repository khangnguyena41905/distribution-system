using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace INVENTORY.API.Abstractions;

[ApiController]
[Route("api/inventory/[controller]")]
public abstract class ApiBaseController : ControllerBase
{
    protected readonly ISender _sender;

    protected ApiBaseController(ISender sender)
    {
        _sender = sender;
    }

    protected ApiBaseController()
    {
    }
}