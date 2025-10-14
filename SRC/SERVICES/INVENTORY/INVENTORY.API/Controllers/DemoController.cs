using Microsoft.AspNetCore.Mvc;

namespace INVENTORY.API.Controllers;

[Route("api/inventory")]
public class DemoController : Controller
{
    // GET
    [HttpGet("Demo")]
    public IActionResult GetDemo()
    {
        return Ok("Inventory works!");
    }
}