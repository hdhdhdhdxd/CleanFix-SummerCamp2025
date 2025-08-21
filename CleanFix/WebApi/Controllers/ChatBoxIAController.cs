using Microsoft.AspNetCore.Mvc;
using WebApi.CoreBot;
using WebApi.Models;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBoxIAController : ControllerBase
    {
        private readonly IBotService _botService;

        public ChatBoxIAController(IBotService botService)
        {
            _botService = botService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MensajeRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Mensaje))
            {
                return BadRequest(new MensajeResponse
                {
                    Success = false,
                    Error = "El campo 'mensaje' es requerido.",
                    Data = null
                });
            }
            var respuesta = await _botService.ProcesarMensajeAsync(request.Mensaje);
            return Ok(new MensajeResponse
            {
                Success = respuesta.Success,
                Error = respuesta.Error,
                Data = respuesta.Data
            });
        }
    }
}
