using Microsoft.AspNetCore.Mvc;
using WebApi.CoreBot;
using WebApi.Models;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/chatboxia")]
    public class ChatBoxIaController : ControllerBase
    {
        private readonly IBotService _botService;

        public ChatBoxIaController(IBotService botService)
        {
            _botService = botService;
        }

        [HttpPost]
        public async Task<ActionResult<MensajeResponse>> Post([FromBody] MensajeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Texto))
                return BadRequest("Texto vacío");

            var respuesta = await _botService.ProcesarMensajeAsync(request.Texto);
            return Ok(new MensajeResponse { Respuesta = respuesta });
        }
    }
}
