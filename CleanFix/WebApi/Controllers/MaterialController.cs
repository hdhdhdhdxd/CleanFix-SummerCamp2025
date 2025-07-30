using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Interfaces;

namespace WebApi.Controllers;
[Route("api/material")]
[ApiController]
public class MaterialController : ControllerBase
{
    private IMaterial _material;

    public MaterialController(IMaterial material)
    {
        _material = material;
    }

    [HttpGet]
    public IActionResult Get()
    {
        // Devuelve un saludo simple
        return Ok(_material.GetAll());
    }
}
