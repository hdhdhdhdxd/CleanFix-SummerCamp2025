using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;
using System.Threading.Tasks;
using CleanFix.Plugins;
using System;
using System.Linq;
using System.Collections.Generic;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/chatboxia")]
    public class ChatBoxIAController : ControllerBase
    {
        private readonly IAssistantService _assistantService;
        private readonly string _connectionString;

        public ChatBoxIAController(IAssistantService assistantService, IConfiguration config)
        {
            _assistantService = assistantService;
            _connectionString = config.GetConnectionString("CleanFixDB");
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
                var dbPlugin = new DBPluginTestPG(_connectionString);
                var empresasResponse = dbPlugin.GetAllEmpresas();
                var materialesResponse = dbPlugin.GetAllMaterials();

                // Buscar empresa por nombre
                var empresa = empresasResponse.Data?.FirstOrDefault(e =>
                    string.Equals(e.Name, request.EmpresaNombre, StringComparison.OrdinalIgnoreCase));
                if (empresa == null)
                    return BadRequest(new FacturaResponse { Success = false, Error = "Empresa no encontrada." });

                // Buscar materiales por nombre
                var materiales = materialesResponse.Data?
                    .Where(m => request.MaterialesNombres.Contains(m.Name, StringComparer.OrdinalIgnoreCase))
                    .ToList() ?? new List<MaterialIa>();
                if (materiales.Count == 0)
                    return BadRequest(new FacturaResponse { Success = false, Error = "No se encontraron materiales." });

                // Generar factura detallada
                decimal iva = 0.21m;
                decimal costeEmpresa = empresa.Price;
                decimal costeMateriales = materiales.Sum(m => m.Cost);
                decimal ivaEmpresa = costeEmpresa * iva;
                decimal ivaMateriales = costeMateriales * iva;
                decimal total = costeEmpresa + ivaEmpresa + costeMateriales + ivaMateriales;

                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"Empresa: {empresa.Name}: COSTE€{costeEmpresa:F2}");
                foreach (var m in materiales)
                {
                    sb.AppendLine($"Material: {m.Name}: COSTE€{m.Cost:F2}");
                }
                sb.AppendLine($"Total factura con IVA: €{total:F2}");

                return Ok(new FacturaResponse { Success = true, Factura = sb.ToString() });
            }
            catch (Exception ex)
            {
                return BadRequest(new FacturaResponse { Success = false, Error = ex.Message });
            }
        }
    }
}
