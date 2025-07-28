using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;

namespace WebApi.Controllers;
[Route("api/empresas")]
[ApiController]
public class EmpresasController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var list = new List<Empresas>
        {
            new Empresas{Id = 1, Nombre = "fontaneriaPaco", Direccion = "CalleDeLosTubos", Telefono= "568394732", Email = "fontaneriaPaco@gmail.com", Tipo ="fontaneria", TiempoTrabajo = new TimeSpan(2, 30, 0), Coste = 50.00M},
            new Empresas{Id = 2, Nombre = "electricidadAntonio", Direccion = "CalleDeLosCables", Telefono= "937849572", Email = "electricidadAntonio@gmail.com", Tipo ="electricidad", TiempoTrabajo = new TimeSpan(3, 0, 0), Coste = 30.00M}
        };
        return Ok(list);
    }
}
