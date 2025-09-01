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

        public class MensajeResponse
        {
            public bool Success { get; set; }
            public string Error { get; set; }
            public object Data { get; set; }
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

            // Limitar el historial a los últimos 3 mensajes
            var historial = request.Historial != null && request.Historial.Count > 3
                ? request.Historial.Skip(request.Historial.Count - 3).ToList()
                : request.Historial;

            var mensajeLower = request.Mensaje.ToLowerInvariant();
            bool esFactura = mensajeLower.Contains("factura") || mensajeLower.Contains("pdf") || mensajeLower.Contains("enviar factura") || mensajeLower.Contains("descargar factura");
            bool pideMateriales = mensajeLower.Contains("material") || mensajeLower.Contains("materiales");
            bool pideEmpresas = mensajeLower.Contains("empresa") || mensajeLower.Contains("empresas");

            // Mensaje ilegible: no contiene palabras clave conocidas
            if (!esFactura && !pideMateriales && !pideEmpresas && request.Mensaje.Length < 20)
            {
                return Ok(new MensajeResponse
                {
                    Success = true,
                    Error = null,
                    Data = new { mensaje = "Lo siento, no entiendo tu mensaje. ¿Puedes reformularlo?" }
                });
            }

            // Si es factura, genera la factura y luego ofrece opciones
            if (esFactura)
            {
                // Aquí podrías generar la factura real y devolverla en la respuesta
                // Para ejemplo, solo devuelvo un mensaje y las opciones
                var pdfUrl = "/api/chatboxia/factura/pdf";
                return Ok(new MensajeResponse
                {
                    Success = true,
                    Error = null,
                    Data = new {
                        mensaje = "Aquí tienes tu factura. ¿Quieres descargarla o recibirla por email?",
                        pdfUrl = pdfUrl,
                        puedeEnviarEmail = true
                    }
                });
            }

            // Llama al servicio con el historial limitado
            var respuesta = await _assistantService.ProcesarMensajeAsync(request.Mensaje, historial);

            // Si pide materiales, filtra la respuesta para evitar empresas
            if (pideMateriales && !esFactura)
            {
                // Si la respuesta contiene la palabra 'empresa' pero no 'material', muestra solo materiales
                if (respuesta.ToLower().Contains("empresa") && !respuesta.ToLower().Contains("material"))
                {
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new { mensaje = "Aquí tienes los materiales disponibles." }
                    });
                }
            }
            // Si pide empresas, filtra la respuesta para evitar materiales
            if (pideEmpresas && !esFactura)
            {
                if (respuesta.ToLower().Contains("material") && !respuesta.ToLower().Contains("empresa"))
                {
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new { mensaje = "Aquí tienes las empresas disponibles." }
                    });
                }
            }

            return Ok(new MensajeResponse
            {
                Success = true,
                Error = null,
                Data = new { mensaje = respuesta }
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
            var pdfBytes = await _facturaPdfService.GenerarFacturaPdfAsync(factura);

            // Log para depuración: tamaño del PDF
            Console.WriteLine($"[FacturaPorGmail] PDF size: {pdfBytes.Length}");

            using var httpClient = new HttpClient();
            using var form = new MultipartFormDataContent();
            // Adjuntar el PDF como archivo binario, igual que en /factura/pdf
            var fileContent = new ByteArrayContent(pdfBytes);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            form.Add(fileContent, "file", $"Factura_{empresa.Name}.pdf");
            // Agregar los metadatos como campos de formulario
            form.Add(new StringContent(request.EmailDestino ?? string.Empty), "email");
            form.Add(new StringContent("Tu factura CleanFix"), "asunto");
            form.Add(new StringContent("Adjuntamos tu factura solicitada."), "mensaje");

            var response = await httpClient.PostAsync("https://hook.eu2.make.com/7pw1u52rkbwmstxlwac79kfwyr56s1y6", form);

            if (response.IsSuccessStatusCode)
                return Ok("Factura enviada correctamente por Gmail (Make). (binario)");
            else
                return StatusCode((int)response.StatusCode, "Error al enviar la factura por Gmail (Make). (binario)");
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
