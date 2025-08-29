using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Services;
using System.Threading.Tasks;
using CleanFix.Plugins;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/chatboxia")]
    public class ChatBoxIAController : ControllerBase
    {
        private readonly IAssistantService _assistantService;
        private readonly string _connectionString;
        private readonly IFacturaPdfService _facturaPdfService;
        private readonly IConfiguration _config;

        public ChatBoxIAController(IAssistantService assistantService, IFacturaPdfService facturaPdfService, IConfiguration config)
        {
            _assistantService = assistantService;
            _facturaPdfService = facturaPdfService;
            _config = config;
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

                // Generar factura estructurada
                decimal iva = 0.21m;
                decimal costeEmpresa = empresa.Price;
                decimal costeMateriales = materiales.Sum(m => m.Cost);
                decimal ivaEmpresa = costeEmpresa * iva;
                decimal ivaMateriales = costeMateriales * iva;
                decimal total = costeEmpresa + ivaEmpresa + costeMateriales + ivaMateriales;

                var factura = new FacturaDetalleDto
                {
                    Empresa = new FacturaEmpresaDto { Nombre = empresa.Name, Coste = costeEmpresa },
                    Materiales = materiales.Select(m => new FacturaMaterialDto { Nombre = m.Name, Coste = m.Cost }).ToList(),
                    TotalConIVA = total
                };

                return Ok(new FacturaResponse { Success = true, Factura = factura });
            }
            catch (Exception ex)
            {
                return BadRequest(new FacturaResponse { Success = false, Error = ex.Message });
            }
        }

        [HttpPost("factura/pdf")]
        public async Task<IActionResult> GenerarFacturaPdf([FromBody] FacturaRequest request)
        {
            var dbPlugin = new DBPluginTestPG(_connectionString);
            var empresasResponse = dbPlugin.GetAllEmpresas();
            var materialesResponse = dbPlugin.GetAllMaterials();
            var empresa = empresasResponse.Data?.FirstOrDefault(e =>
                string.Equals(e.Name, request.EmpresaNombre, StringComparison.OrdinalIgnoreCase));
            var materiales = materialesResponse.Data?
                .Where(m => request.MaterialesNombres.Contains(m.Name, StringComparer.OrdinalIgnoreCase))
                .ToList() ?? new List<MaterialIa>();
            if (empresa == null || materiales.Count == 0)
                return BadRequest("Empresa o materiales no encontrados");
            decimal iva = 0.21m;
            decimal costeEmpresa = empresa.Price;
            decimal costeMateriales = materiales.Sum(m => m.Cost);
            decimal ivaEmpresa = costeEmpresa * iva;
            decimal ivaMateriales = costeMateriales * iva;
            decimal total = costeEmpresa + ivaEmpresa + costeMateriales + ivaMateriales;
            var factura = new FacturaDetalleDto
            {
                Empresa = new FacturaEmpresaDto { Nombre = empresa.Name, Coste = costeEmpresa },
                Materiales = materiales.Select(m => new FacturaMaterialDto { Nombre = m.Name, Coste = m.Cost }).ToList(),
                TotalConIVA = total
            };
            var pdfBytes = await _facturaPdfService.GenerarFacturaPdfAsync(factura);
            return File(pdfBytes, "application/pdf", $"Factura_{empresa.Name}.pdf");
        }

        [HttpPost("factura/gmail")]
        public async Task<IActionResult> EnviarFacturaPorGmail([FromBody] FacturaPdfRequest request)
        {
            var dbPlugin = new DBPluginTestPG(_connectionString);
            var empresasResponse = dbPlugin.GetAllEmpresas();
            var materialesResponse = dbPlugin.GetAllMaterials();
            var empresa = empresasResponse.Data?.FirstOrDefault(e =>
                string.Equals(e.Name, request.EmpresaNombre, StringComparison.OrdinalIgnoreCase));
            var materiales = materialesResponse.Data?
                .Where(m => request.MaterialesNombres.Contains(m.Name, StringComparer.OrdinalIgnoreCase))
                .ToList() ?? new List<MaterialIa>();
            if (empresa == null || materiales.Count == 0)
                return BadRequest("Empresa o materiales no encontrados");
            decimal iva = 0.21m;
            decimal costeEmpresa = empresa.Price;
            decimal costeMateriales = materiales.Sum(m => m.Cost);
            decimal ivaEmpresa = costeEmpresa * iva;
            decimal ivaMateriales = costeMateriales * iva;
            decimal total = costeEmpresa + ivaEmpresa + costeMateriales + ivaMateriales;
            var factura = new FacturaDetalleDto
            {
                Empresa = new FacturaEmpresaDto { Nombre = empresa.Name, Coste = costeEmpresa },
                Materiales = materiales.Select(m => new FacturaMaterialDto { Nombre = m.Name, Coste = m.Cost }).ToList(),
                TotalConIVA = total
            };
            // Generar el PDF igual que en /factura/pdf
            var pdfBytes = await _facturaPdfService.GenerarFacturaPdfAsync(factura);

            var payload = new
            {
                email = request.EmailDestino,
                facturaPdf = Convert.ToBase64String(pdfBytes),
                asunto = "Tu factura CleanFix",
                mensaje = "Adjuntamos tu factura solicitada."
            };

            using var httpClient = new HttpClient();
            var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://hook.eu2.make.com/7pw1u52rkbwmstxlwac79kfwyr56s1y6", content);

            if (response.IsSuccessStatusCode)
                return Ok("Factura enviada correctamente por Gmail (Make).");
            else
                return StatusCode((int)response.StatusCode, "Error al enviar la factura por Gmail (Make).");
        }

        [HttpPost("factura/recomendar")]
        public IActionResult RecomendarAlternativas([FromBody] FacturaRequest request)
        {
            var dbPlugin = new DBPluginTestPG(_connectionString);
            var empresasResponse = dbPlugin.GetAllEmpresas();
            var materialesResponse = dbPlugin.GetAllMaterials();
            var recomendador = new RecomendacionService();
            var alternativas = recomendador.RecomendarAlternativas(empresasResponse.Data, materialesResponse.Data, request.EmpresaNombre, request.MaterialesNombres);
            return Ok(alternativas);
        }

        [HttpPost("incidencia")]
        public async Task<IActionResult> ReportarIncidencia([FromBody] IncidenciaRequest request, [FromServices] IIncidenciaService incidenciaService)
        {
            await incidenciaService.ReportarIncidenciaAsync(request.Usuario, request.Descripcion, request.Tipo, request.Referencia);
            return Ok("Incidencia reportada correctamente");
        }
    }
}
