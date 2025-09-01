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

            // --- FLUJO: Descargar factura PDF tras mostrarla ---
            if (mensajeLower.Contains("descargar") && historial != null && historial.Any(h => h.ToLower().Contains("factura")))
            {
                // Se asume que la última factura generada es la que quiere descargar
                // (En un sistema real, se debería guardar el contexto de la última factura generada)
                return Ok(new MensajeResponse
                {
                    Success = true,
                    Error = null,
                    Data = new {
                        mensaje = "Puedes descargar tu factura aquí: [Descargar PDF](/api/chatboxia/factura/pdf)",
                        pdfUrl = "/api/chatboxia/factura/pdf"
                    }
                });
            }

            // --- FLUJO: Enviar por Gmail ---
            if ((mensajeLower.Contains("enviar por gmail") || mensajeLower.Contains("enviamela por correo") || mensajeLower.Contains("enviármela por correo") || mensajeLower.Contains("enviamela al correo") || mensajeLower.Contains("enviarmela al correo")) && historial != null && historial.Any(h => h.ToLower().Contains("factura")))
            {
                // Pide el email al usuario
                return Ok(new MensajeResponse
                {
                    Success = true,
                    Error = null,
                    Data = new {
                        mensaje = "Por favor, indícame tu correo electrónico para enviarte la factura por Gmail.",
                        requiereEmail = true
                    }
                });
            }

            // --- FLUJO: El usuario introduce un email tras pedir envío por Gmail ---
            if (historial != null && historial.Any(h => h.ToLower().Contains("enviarte la factura por gmail")) &&
                !string.IsNullOrWhiteSpace(request.Mensaje) &&
                System.Text.RegularExpressions.Regex.IsMatch(request.Mensaje.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                // Recuperar la última factura del historial (en un sistema real, se debería guardar el contexto de la factura)
                // Aquí solo se simula el flujo
                var emailDestino = request.Mensaje.Trim();
                // Se requiere que el frontend envíe los datos de la factura en el siguiente mensaje para poder enviarla correctamente
                return Ok(new MensajeResponse
                {
                    Success = true,
                    Error = null,
                    Data = new {
                        mensaje = $"Factura enviada correctamente a {emailDestino} (simulado).",
                        email = emailDestino
                    }
                });
            }

            // --- FILTRO: empresas de tipo X ---
            if ((mensajeLower.Contains("empresa") || mensajeLower.Contains("empresas")) && mensajeLower.Contains("tipo"))
            {
                var tipoMatch = System.Text.RegularExpressions.Regex.Match(request.Mensaje, @"tipo\s*(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (tipoMatch.Success && int.TryParse(tipoMatch.Groups[1].Value, out int tipoId))
                {
                    var dbPlugin = new DBPluginTestPG(_connectionString);
                    var empresasResponse = dbPlugin.GetAllEmpresas();
                    var empresas = empresasResponse.Data?.Where(e => e.IssueTypeId == tipoId).ToList() ?? new List<CompanyIa>();
                    if (empresas.Count == 0)
                    {
                        return Ok(new MensajeResponse
                        {
                            Success = true,
                            Error = null,
                            Data = new { mensaje = $"No se encontraron empresas de tipo {tipoId}." }
                        });
                    }
                    var listado = string.Join(", ", empresas.Select(e => $"{e.Name} (ID: {e.Number}, Precio: €{e.Price:F2}, Tipo: {e.IssueTypeId})"));
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new { mensaje = $"Empresas de tipo {tipoId}: {listado}" }
                    });
                }
            }

            // --- 1. TODAS LAS EMPRESAS: Mensaje personalizado ---
            if (
                (mensajeLower.Contains("todas las empresas") || mensajeLower.Trim() == "empresas" || mensajeLower.Contains("lista de empresas") || mensajeLower.Contains("dame todas las empresas"))
                && !esFactura
            )
            {
                return Ok(new MensajeResponse
                {
                    Success = true,
                    Error = null,
                    Data = new { mensaje = "No puedo darte todas las empresas, pero puedo mostrarte las de un tipo concreto. ¿Qué tipo quieres?" }
                });
            }

            // --- PRIORIDAD: FACTURA ---
            if (esFactura)
            {
                string empresaNombre = null;
                var materialesNombres = new List<string>();

                var empresaRegex = System.Text.RegularExpressions.Regex.Match(request.Mensaje, @"empresa\s+([a-zA-Z0-9áéíóúÁÉÍÓÚüÜñÑ]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (empresaRegex.Success)
                {
                    empresaNombre = empresaRegex.Groups[1].Value.Trim();
                }

                var materialesRegex = System.Text.RegularExpressions.Regex.Match(request.Mensaje, @"material(?:es)?\s+([a-zA-Z0-9áéíóúÁÉÍÓÚüÜñÑ ,y]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (materialesRegex.Success)
                {
                    var nombres = materialesRegex.Groups[1].Value
                        .Split(new[] {',', 'y'}, StringSplitOptions.RemoveEmptyEntries)
                        .Select(x => x.Trim())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .ToList();
                    materialesNombres.AddRange(nombres);
                }

                if (string.IsNullOrEmpty(empresaNombre))
                {
                    var respuesta = await _assistantService.ProcesarMensajeAsync(request.Mensaje, historial);
                    var pdfUrl = "/api/chatboxia/factura/pdf";
                    var mensajeFinal = respuesta;
                    if (!string.IsNullOrWhiteSpace(respuesta) && respuesta.ToLower().Contains("factura"))
                        mensajeFinal += "\n\n¿Quieres descargarla en formato PDF o prefieres que te la envíe por correo?";
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new {
                            mensaje = mensajeFinal,
                            pdfUrl = pdfUrl,
                            puedeEnviarEmail = true,
                            sugerencia = "¿Quieres descargarla en formato PDF o prefieres que te la envíe por correo?"
                        }
                    });
                }

                string Normalizar(string s) => s.Trim().ToLower().Replace("empresa", "").Trim();
                var dbPlugin = new DBPluginTestPG(_connectionString);
                var empresasResponse = dbPlugin.GetAllEmpresas();
                var empresa = empresasResponse.Data?.FirstOrDefault(e =>
                    string.Equals(e.Name.Trim(), empresaNombre, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(Normalizar(e.Name), Normalizar(empresaNombre), StringComparison.OrdinalIgnoreCase)
                );
                if (empresa == null)
                {
                    return Ok(new MensajeResponse
                    {
                        Success = false,
                        Error = null,
                        Data = new { mensaje = $"No se encontró ninguna empresa con el nombre '{empresaNombre}'." }
                    });
                }

                // Si hay empresa pero NO hay materiales, mostrar factura estructurada solo con empresa y sugerir materiales
                if (materialesNombres.Count == 0)
                {
                    var materialesResponse = dbPlugin.GetAllMaterials();
                    var sugeridos = materialesResponse.Data?.Take(4).ToList() ?? new List<MaterialIa>();
                    var factura = new FacturaDetalleDto
                    {
                        Empresa = new FacturaEmpresaDto { Nombre = empresa.Name, Coste = empresa.Price },
                        Materiales = new List<FacturaMaterialDto>(),
                        TotalConIVA = Math.Round(empresa.Price * 1.21m, 2)
                    };
                    var desglose = FormatearFactura(factura);
                    var sugerencia = "No has seleccionado materiales, te recomiendo estos: " + string.Join(", ", sugeridos.Select(m => $"{m.Name} - €{m.Cost:F2}"));
                    var mensajeFinal = desglose + "\n" + sugerencia + "\n\n¿Quieres descargarla en formato PDF o prefieres que te la envíe por correo?";
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new {
                            mensaje = mensajeFinal,
                            sugerencia = "¿Quieres descargarla en formato PDF o prefieres que te la envíe por correo?"
                        }
                    });
                }

                // Llama a la lógica real de generación de factura
                var facturaRequest = new FacturaRequest { EmpresaNombre = empresa.Name, MaterialesNombres = materialesNombres };
                var facturaResult = GenerarFactura(facturaRequest) as OkObjectResult;
                if (facturaResult?.Value is FacturaResponse facturaResponse && facturaResponse.Success && facturaResponse.Factura != null)
                {
                    var desglose = FormatearFactura(facturaResponse.Factura);
                    var pdfUrl = "/api/chatboxia/factura/pdf";
                    var mensajeFinal = desglose + "\n\n¿Quieres descargarla en formato PDF o prefieres que te la envíe por correo?";
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new {
                            mensaje = mensajeFinal,
                            pdfUrl = pdfUrl,
                            puedeEnviarEmail = true,
                            sugerencia = "¿Quieres descargarla en formato PDF o prefieres que te la envíe por correo?"
                        }
                    });
                }
                else
                {
                    var respuesta = await _assistantService.ProcesarMensajeAsync(request.Mensaje, historial);
                    var pdfUrl = "/api/chatboxia/factura/pdf";
                    var mensajeFinal = respuesta;
                    if (!string.IsNullOrWhiteSpace(respuesta) && respuesta.ToLower().Contains("factura"))
                        mensajeFinal += "\n\n¿Quieres descargarla en formato PDF o prefieres que te la envíe por correo?";
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new {
                            mensaje = mensajeFinal,
                            pdfUrl = pdfUrl,
                            puedeEnviarEmail = true,
                            sugerencia = "¿Quieres descargarla en formato PDF o prefieres que te la envíe por correo?"
                        }
                    });
                }
            }

            // --- RESTO: TODO AL ASSISTANTSERVICE ---
            var respuestaGeneral = await _assistantService.ProcesarMensajeAsync(request.Mensaje, historial);
            return Ok(new MensajeResponse
            {
                Success = true,
                Error = null,
                Data = new { mensaje = respuestaGeneral }
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

        // Formatea el desglose de la factura real
        private string FormatearFactura(FacturaDetalleDto factura)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("==============================");
            sb.AppendLine("         FACTURA CLEANFIX      ");
            sb.AppendLine("==============================");
            sb.AppendLine($"Empresa: {factura.Empresa.Nombre}");
            sb.AppendLine($"Coste empresa:         €{factura.Empresa.Coste,8:F2}");
            sb.AppendLine("------------------------------");
            if (factura.Materiales != null && factura.Materiales.Count > 0)
            {
                sb.AppendLine("Materiales:");
                foreach (var m in factura.Materiales)
                {
                    sb.AppendLine($"  - {m.Nombre,-20} €{m.Coste,8:F2}");
                }
                sb.AppendLine("------------------------------");
                decimal iva = 0.21m;
                decimal ivaEmpresa = factura.Empresa.Coste * iva;
                decimal ivaMateriales = factura.Materiales?.Sum(m => m.Coste) * iva ?? 0;
                decimal totalIva = ivaEmpresa + ivaMateriales;
                sb.AppendLine($"IVA (21%):            €{totalIva,8:F2}");
                sb.AppendLine($"TOTAL CON IVA:        €{factura.TotalConIVA,8:F2}");
            }
            else
            {
                // Sugerir materiales si no hay ninguno en la factura
                var dbPlugin = new DBPluginTestPG(_connectionString);
                var materialesResponse = dbPlugin.GetAllMaterials();
                var sugeridos = materialesResponse.Data?.Take(4).ToList() ?? new List<MaterialIa>();
                sb.AppendLine("Debes añadir algún material, te recomiendo estos:");
                foreach (var m in sugeridos)
                {
                    sb.AppendLine($"  - {m.Name,-20} €{m.Cost,8:F2}");
                }
            }
            sb.AppendLine("==============================");
            return sb.ToString();
        }
    }
}
