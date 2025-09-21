using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IDENTITY.API.Abstractions;
using IDENTITY.APPLICATION.Features.Accounts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IDENTITY.API.Controllers.Identities;

public class AuthController : ApiBaseController
{
    public AuthController(ISender sender):base(sender)
    {
    }
    // [HttpPost("login")]
    // public async Task<IActionResult> Login([FromForm] string username, [FromForm] string password)
    // {
    //     var result = await _sender.Send(new CheckAccountQuery(username, password));
    //
    //     if (!result.IsSuccess)
    //         return BadRequest(result.Error);
    //     
    //     var appUser = result.Value.AppUser;
    //     
    //     var claims = new List<Claim>
    //     {
    //         new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString())
    //     };
    //
    //     foreach (var claim in appUser.Claims)
    //     {
    //         claims.Add(new Claim(claim.ClaimType, claim.ClaimValue));
    //     }
    //     
    //     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YOUR_SECRET_KEY"));
    //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
    //
    //     var token = new JwtSecurityToken(
    //         issuer: "identity-server",
    //         audience: "api-gateway",
    //         claims: claims,
    //         expires: DateTime.Now.AddHours(2),
    //         signingCredentials: creds
    //     );
    //
    //     return Ok(new
    //     {
    //         access_token = new JwtSecurityTokenHandler().WriteToken(token)
    //     });
    // }

}