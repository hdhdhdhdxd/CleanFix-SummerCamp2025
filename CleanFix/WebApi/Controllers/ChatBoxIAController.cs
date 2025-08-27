using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;
using System.Threading.Tasks;
using CleanFix.Plugins;
using System;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/chatboxia")]
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

        [HttpPost("factura")]
        public IActionResult GenerarFactura([FromBody] FacturaRequest request)
        {
            try
            {
                var plugin = new FacturaPluginTestPG();
                string factura = plugin.GenerarFactura(request.Empresa, request.Materiales);
                return Ok(new FacturaResponse { Success = true, Factura = factura });
            }
            catch (Exception ex)
            {
                return BadRequest(new FacturaResponse { Success = false, Error = ex.Message });
            }
        }
    }
}
