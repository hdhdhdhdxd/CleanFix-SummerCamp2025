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

            // --- NUEVO: Manejo de empresas por tipo ---
            if (pideEmpresas)
            {
                // Obtener todos los tipos de problema (IssueType) para asociar nombre a tipo
                var dbPlugin = new DBPluginTestPG(_connectionString);
                var issueTypeRepo = new Infrastructure.Repositories.IssueTypeRepository(_connectionString);
                var issueTypes = await issueTypeRepo.GetAllAsync();

                // Detectar si pide todas las empresas de un tipo concreto por nombre (ej: "problema electrico")
                var tipoMatch = System.Text.RegularExpressions.Regex.Match(request.Mensaje, @"tipo\\s*([a-zA-Z0-9áéíóúÁÉÍÓÚüÜñÑ]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                int? tipoInt = null;
                string tipoTexto = null;
                if (tipoMatch.Success)
                {
                    var tipo = tipoMatch.Groups[1].Value.Trim();
                    tipoTexto = tipo;
                    if (int.TryParse(tipo, out int tipoNum))
                    {
                        tipoInt = tipoNum;
                    }
                    else
                    {
                        // Buscar por nombre de tipo (IssueType)
                        var tipoPorNombre = issueTypes.FirstOrDefault(it => it.Name.Equals(tipo, StringComparison.OrdinalIgnoreCase));
                        if (tipoPorNombre != null)
                            tipoInt = tipoPorNombre.Id;
                    }
                }
                else
                {
                    // Buscar por nombre de tipo en todo el mensaje
                    var tipoPorNombre = issueTypes.FirstOrDefault(it => request.Mensaje.Contains(it.Name, StringComparison.OrdinalIgnoreCase));
                    if (tipoPorNombre != null)
                    {
                        tipoInt = tipoPorNombre.Id;
                        tipoTexto = tipoPorNombre.Name;
                    }
                }

                if (tipoInt.HasValue)
                {
                    var empresasResponse = dbPlugin.GetAllEmpresas();
                    var empresasFiltradas = empresasResponse.Data?.Where(e => e.Type == tipoInt.Value).ToList() ?? new List<CompanyIa>();
                    if (empresasFiltradas.Count == 0)
                    {
                        return Ok(new MensajeResponse
                        {
                            Success = true,
                            Error = null,
                            Data = new { mensaje = $"No se encontraron empresas del tipo '{tipoTexto ?? tipoInt.Value.ToString()}'." }
                        });
                    }
                    var listado = string.Join(", ", empresasFiltradas.Select(e => $"{e.Name} (Precio: €{e.Price:F2})"));
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new { mensaje = $"Empresas de tipo '{tipoTexto ?? tipoInt.Value.ToString()}': {listado}" }
                    });
                }
                // Si pide todas las empresas sin tipo
                if (mensajeLower.Contains("todas las empresas") || mensajeLower.Trim() == "empresas" || mensajeLower.Trim() == "dame todas las empresas")
                {
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new { mensaje = "No puedo darte todas las empresas pero sí puedo darte las que sean de un tipo concreto. ¿De qué tipo quieres que te muestre las empresas?" }
                    });
                }
                // Si no se reconoce el tipo
                return Ok(new MensajeResponse
                {
                    Success = true,
                    Error = null,
                    Data = new { mensaje = "El tipo debe ser un número o un nombre de problema válido (por ejemplo: 'eléctrico', 'fontanería', etc.)." }
                });
            }
            // --- FIN NUEVO ---

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

            // Si es factura, intenta extraer empresa y materiales y usa la lógica real
            if (esFactura)
            {
                // Extracción simple de empresa y materiales por ID o nombre (mejorable con NLP)
                string empresaNombre = null;
                var materialesNombres = new List<string>();
                var empresaMatch = System.Text.RegularExpressions.Regex.Match(request.Mensaje, @"empresa\s*(\d+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (empresaMatch.Success)
                {
                    empresaNombre = $"Empresa {empresaMatch.Groups[1].Value}";
                }
                else
                {
                    // Busca por nombre si no hay ID
                    var nombreMatch = System.Text.RegularExpressions.Regex.Match(request.Mensaje, @"empresa ([a-zA-Z0-9 ]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                    if (nombreMatch.Success)
                        empresaNombre = nombreMatch.Groups[1].Value.Trim();
                }
                // Materiales por nombre o id
                var materialesMatches = System.Text.RegularExpressions.Regex.Matches(request.Mensaje, @"material(?:es)?\s*([a-zA-Z0-9 ]+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                foreach (System.Text.RegularExpressions.Match mat in materialesMatches)
                {
                    var nombre = mat.Groups[1].Value.Trim();
                    if (!string.IsNullOrEmpty(nombre))
                        materialesNombres.Add(nombre);
                }

                // Si no se encuentra empresa, fallback a AssistantService
                if (string.IsNullOrEmpty(empresaNombre))
                {
                    var respuesta = await _assistantService.ProcesarMensajeAsync(request.Mensaje, historial);
                    var pdfUrl = "/api/chatboxia/factura/pdf";
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new {
                            mensaje = respuesta,
                            pdfUrl = pdfUrl,
                            puedeEnviarEmail = true,
                            sugerencia = "¿Quieres descargarla o recibirla por email?"
                        }
                    });
                }

                // Si hay empresa pero NO hay materiales, sugerir materiales y NO generar factura
                if (materialesNombres.Count == 0)
                {
                    var dbPlugin = new DBPluginTestPG(_connectionString);
                    var materialesResponse = dbPlugin.GetAllMaterials();
                    var sugeridos = materialesResponse.Data?.Take(4).ToList() ?? new List<MaterialIa>();
                    var sugerencia = $"No has seleccionado materiales, te recomiendo estos: Materiales: " + string.Join(", ", sugeridos.Select(m => $"{m.Name} - €{m.Cost:F2}"));
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new {
                            mensaje = $"Factura:\nEmpresa: {empresaNombre}\n{sugerencia}"
                        }
                    });
                }

                // Llama a la lógica real de generación de factura
                var facturaRequest = new FacturaRequest { EmpresaNombre = empresaNombre, MaterialesNombres = materialesNombres };
                var facturaResult = GenerarFactura(facturaRequest) as OkObjectResult;
                if (facturaResult?.Value is FacturaResponse facturaResponse && facturaResponse.Success && facturaResponse.Factura != null)
                {
                    var desglose = FormatearFactura(facturaResponse.Factura);
                    var pdfUrl = "/api/chatboxia/factura/pdf";
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new {
                            mensaje = desglose,
                            pdfUrl = pdfUrl,
                            puedeEnviarEmail = true,
                            sugerencia = "¿Quieres descargarla o recibirla por email?"
                        }
                    });
                }
                else
                {
                    // Si no se puede generar, fallback
                    var respuesta = await _assistantService.ProcesarMensajeAsync(request.Mensaje, historial);
                    var pdfUrl = "/api/chatboxia/factura/pdf";
                    return Ok(new MensajeResponse
                    {
                        Success = true,
                        Error = null,
                        Data = new {
                            mensaje = respuesta,
                            pdfUrl = pdfUrl,
                            puedeEnviarEmail = true,
                            sugerencia = "¿Quieres descargarla o recibirla por email?"
                        }
                    });
                }
            }

            // Llama al servicio con el historial limitado
            var respuestaGeneral = await _assistantService.ProcesarMensajeAsync(request.Mensaje, historial);

            // Si pide materiales, filtra la respuesta para evitar empresas
            if (pideMateriales && !esFactura)
            {
                if (respuestaGeneral.ToLower().Contains("empresa") && !respuestaGeneral.ToLower().Contains("material"))
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
                if (respuestaGeneral.ToLower().Contains("material") && !respuestaGeneral.ToLower().Contains("empresa"))
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
            sb.AppendLine($"Factura:");
            sb.AppendLine($"Empresa: {factura.Empresa.Nombre} - €{factura.Empresa.Coste:F2}");
            if (factura.Materiales != null && factura.Materiales.Count > 0)
            {
                sb.Append("Materiales: ");
                sb.Append(string.Join(", ", factura.Materiales.Select(m => $"{m.Nombre} - €{m.Coste:F2}")));
                sb.AppendLine();
                decimal iva = 0.21m;
                decimal ivaEmpresa = factura.Empresa.Coste * iva;
                decimal ivaMateriales = factura.Materiales?.Sum(m => m.Coste) * iva ?? 0;
                decimal totalIva = ivaEmpresa + ivaMateriales;
                sb.AppendLine($"IVA: €{totalIva:F2}");
                sb.AppendLine($"Total con IVA: €{factura.TotalConIVA:F2}");
            }
            else
            {
                // Sugerir materiales si no hay ninguno en la factura
                var dbPlugin = new DBPluginTestPG(_connectionString);
                var materialesResponse = dbPlugin.GetAllMaterials();
                var sugeridos = materialesResponse.Data?.Take(4).ToList() ?? new List<MaterialIa>();
                sb.Append("Debes añadir algún material, te recomiendo estos: Materiales: ");
                sb.Append(string.Join(", ", sugeridos.Select(m => $"{m.Name} - €{m.Cost:F2}")));
            }
            return sb.ToString();
        }
    }
}
