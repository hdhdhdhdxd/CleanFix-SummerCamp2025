using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBoxIAController : ControllerBase
    {
        private readonly IAssistantService _assistantService;

        public ChatBoxIAController(IAssistantService assistantService)
        {
            _assistantService = assistantService;
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
            var respuesta = await _assistantService.ProcesarMensajeAsync(request.Mensaje);
            return Ok(new MensajeResponse
            {
                Success = true,
                Error = null,
                Data = respuesta
            });
        }
    }
}
