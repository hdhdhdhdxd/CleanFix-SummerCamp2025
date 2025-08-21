using Microsoft.AspNetCore.Mvc;
using WebApi.CoreBot.Models;

namespace WebApi.CoreBot.Controllers
{
    [ApiController]
    [Route("api/chatboxia")]
    public class BotController : ControllerBase
    {
        private readonly IBotService _botService;

        public BotController(IBotService botService)
        {
            _botService = botService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] string mensaje)
        {
            var resultado = await _botService.ProcesarMensajeAsync(mensaje);

            if (!resultado.Success)
                return BadRequest(resultado);

            return Ok(resultado);
        }
    }
}
