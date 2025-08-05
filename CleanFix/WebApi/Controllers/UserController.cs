using Microsoft.AspNetCore.Mvc;
using WebApi.Entidades;

namespace WebApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    [HttpPost]
    public IActionResult CrearUsuario([FromBody] User user)
    {
        if(user == null || string.IsNullOrEmpty(user.Name))
        {
            return BadRequest("El nombre del usuario es obligatorio.");
        }
        // Aquí podrías agregar lógica para crear un usuario
        // Por ahora, simplemente devolvemos el nombre recibido
        return Ok(new { mensaje = "Usuario creado correctamente", user });
    }


}
