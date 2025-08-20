using Microsoft.AspNetCore.Mvc;
using WebApi.Servicios;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/chatboxia")]
    public class ChatBoxIAController : ControllerBase
    {
        private readonly BotService _botService;

        public ChatBoxIAController(BotService botService)
        {
            _botService = botService;
        }

        [HttpPost]
        public IActionResult Post([FromBody] string mensaje)
        {
            if (string.IsNullOrWhiteSpace(mensaje))
                return BadRequest("El mensaje no puede estar vacío.");

            var respuesta = _botService.ProcesarMensaje(mensaje);
            return Ok(new { respuesta });
        }
    }
}

