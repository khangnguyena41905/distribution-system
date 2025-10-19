using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IDENTITY.API.Abstractions;
using IDENTITY.APPLICATION.Features.Identities.AppUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IDENTITY.API.Controllers.Identities;

public class AuthController : ApiBaseController
{
    public AuthController(ISender sender):base(sender)
    {
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
            return Unauthorized(result.Error.Message);

        return Ok(result.Value);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Login([FromBody] RegisterAppUserCommand command)
    {
        var result = await _sender.Send(command);
        if (result.IsFailure)
            return Unauthorized(result.Error.Message);
    
        return Ok(result.Value);
    }
}