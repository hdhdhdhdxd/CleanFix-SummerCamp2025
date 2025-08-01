using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("api/usuario")]
[ApiController]

public class UsuarioController : ControllerBase
{
    [HttpPost]
    public IActionResult CrearUsuario([FromBody] Usuario usuario)
    {         if (usuario == null)
        {
            return BadRequest("El usuario no puede ser nulo.");
        }

        // Aquí podrías agregar lógica para guardar el usuario en la base de datos
        // Por ejemplo, usando un DbContext para interactuar con la base de datos

        return Ok(new { mensaje = "Usuario creado correctamente", usuario });
    }
}
