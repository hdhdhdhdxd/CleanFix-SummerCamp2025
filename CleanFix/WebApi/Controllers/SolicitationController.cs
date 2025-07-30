using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Controllers;
[Route("api/solicitation")]
[ApiController]
public class SolicitationController : ControllerBase
{
    private ISolicitation _solicitation;

    public SolicitationController(ISolicitation solicitation)
    {
        _solicitation = solicitation;
    }

    [HttpGet]
    public IActionResult Get()
    {
        // Devuelve un saludo simple
        return Ok(_solicitation.GetAll());
    }
}
