using Microsoft.AspNetCore.Mvc;
using WebApi.CoreBot;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/chatboxiacontroller")]
    public class BotController : ControllerBase
    {
        private readonly IBotService _botService;

        public BotController(IBotService botService)
        {
            _botService = botService;
        }

        [HttpPost("mensaje")]
        public async Task<IActionResult> ProcesarMensaje([FromBody] string mensaje)
        {
            var respuesta = await _botService.ProcesarMensajeAsync(mensaje);
            return Ok(respuesta); // ASP.NET Core serializa automáticamente como JSON
        }
    }
}
