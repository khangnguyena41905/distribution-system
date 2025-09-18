using Microsoft.AspNetCore.Mvc;

namespace IDENTITY.API.Controllers.Identities;

public class AuthController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}