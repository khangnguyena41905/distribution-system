using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IDENTITY.API.Abstractions;

[ApiController]
[Route("api/identity/[controller]")]
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