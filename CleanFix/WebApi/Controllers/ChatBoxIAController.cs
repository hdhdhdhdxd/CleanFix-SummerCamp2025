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

        [HttpPost("factura/email")]
        public async Task<IActionResult> EnviarFacturaPorEmail([FromBody] FacturaPdfRequest request)
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

            // Leer configuración SMTP de appsettings/secrets
            var smtpHost = _config["Email:SmtpHost"];
            var smtpPort = int.Parse(_config["Email:SmtpPort"]);
            var smtpUser = _config["Email:SmtpUser"];
            var smtpPass = _config["Email:SmtpPass"];
            var from = _config["Email:From"];

            var smtpClient = new SmtpClient(smtpHost)
            {
                Port = smtpPort,
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true,
            };
            var mail = new MailMessage(from, request.EmailDestino)
            {
                Subject = "Factura CleanFixBot",
                Body = "Adjuntamos su factura en PDF.",
            };
            mail.Attachments.Add(new Attachment(new System.IO.MemoryStream(pdfBytes), $"Factura_{empresa.Name}.pdf"));
            await smtpClient.SendMailAsync(mail);
            return Ok("Factura enviada por email");
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
