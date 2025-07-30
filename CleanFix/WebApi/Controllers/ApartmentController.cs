using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Controllers;
[Route("api/apartamento")]
[ApiController]
public class ApartmentController : ControllerBase
{
    private IApartment _apartment;

    public ApartmentController(IApartment apartment)
    {
        _apartment = apartment;
    }

    [HttpGet]
    public IActionResult Get()
    {
        // Devuelve un saludo simple
        return Ok(_apartment.GetAll());
    }
}
