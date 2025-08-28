using CleanFix.Plugins;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;
using Infrastructure.Repositories;

namespace WebApi.CoreBot
{
    /// <summary>
    /// Servicio principal del bot CleanFixBot. Gestiona la interpretación de mensajes y la delegación a plugins.
    /// </summary>
    public class CleanFixBotService : IBotService
    {
        private readonly Dictionary<string, IPlugin> _plugins;
        private readonly IClasificadorIntencion _clasificador;
        private readonly string _connectionString;
        private readonly IssueTypeRepository _issueTypeRepository;

        public CleanFixBotService(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("CleanFixDB");

            _plugins = new Dictionary<string, IPlugin>
            {
                { "factura", new FacturaPluginTestPG() },
                { "db", new DBPluginTestPG(_connectionString) }
            };

            _clasificador = new ClasificadorIntencion();
            _issueTypeRepository = new IssueTypeRepository(_connectionString);
        }

        /// <summary>
        /// Procesa el mensaje recibido, interpreta la intención y delega la consulta al plugin adecuado.
        /// </summary>
        public async Task<PluginRespuesta> ProcesarMensajeAsync(string mensaje)
        {
            var intencion = _clasificador.Clasificar(mensaje);

            // Obtener todos los tipos de problema
            var issueTypes = await _issueTypeRepository.GetAllAsync();
            int? issueTypeIdFromName = ConsultaParser.ExtraerIssueTypeIdPorNombre(mensaje, issueTypes);

            // Si la intención es consultar datos, parsea la consulta natural
            if (intencion == IntencionUsuario.ConsultarDatos)
            {
                int? tipoEmpresa = issueTypeIdFromName ?? ConsultaParser.ExtraerTipo(mensaje, "empresa");
                int? tipoMaterial = issueTypeIdFromName ?? ConsultaParser.ExtraerTipo(mensaje, "material");
                bool masBarato = ConsultaParser.SolicitaMasBarato(mensaje);
                bool masCaro = ConsultaParser.SolicitaMasCaro(mensaje);
                bool disponibles = ConsultaParser.SolicitaDisponibles(mensaje);
                bool todosMateriales = ConsultaParser.SolicitaTodosMateriales(mensaje);

                string consulta = "";
                if (mensaje.Contains("empresa"))
                {
                    consulta = "empresas";
                    if (tipoEmpresa.HasValue) consulta += $" tipo={tipoEmpresa.Value}";
                    if (masBarato) consulta += " más barata";
                    if (masCaro) consulta += " más cara";
                }
                else if (mensaje.Contains("material"))
                {
                    consulta = "materiales";
                    if (tipoMaterial.HasValue) consulta += $" tipo={tipoMaterial.Value}";
                    if (masBarato) consulta += " más barato";
                    if (masCaro) consulta += " más caro";
                    if (disponibles) consulta += " disponibles";
                    if (todosMateriales) consulta += " todos";
                }
                else
                {
                    return new PluginRespuesta { Success = false, Error = "No se reconoce la consulta." };
                }

                return await _plugins["db"].EjecutarAsync(consulta);
            }

            // Si la intención es generar factura, permite buscar por nombre de empresa y materiales o por tipo/criterio
            if (intencion == IntencionUsuario.GenerarFactura)
            {
                var dbPlugin = new DBPluginTestPG(_connectionString);
                var empresasResponse = dbPlugin.GetAllEmpresas();
                var materialesResponse = dbPlugin.GetAllMaterials();

                // Extraer nombre de empresa y materiales del mensaje
                string nombreEmpresa = ConsultaParser.ExtraerNombreEmpresa(mensaje);
                List<string> nombresMateriales = ConsultaParser.ExtraerNombresMateriales(mensaje);

                // Extraer tipo y criterios para materiales
                int? tipoMaterial = issueTypeIdFromName ?? ConsultaParser.ExtraerTipo(mensaje, "material");
                bool masBarato = ConsultaParser.SolicitaMasBarato(mensaje);
                bool masCaro = ConsultaParser.SolicitaMasCaro(mensaje);

                CompanyIa empresa = null;
                if (!string.IsNullOrWhiteSpace(nombreEmpresa))
                {
                    empresa = empresasResponse.Data?.FirstOrDefault(e =>
                        string.Equals(e.Name, nombreEmpresa, System.StringComparison.OrdinalIgnoreCase));
                }
                else if (tipoMaterial.HasValue)
                {
                    empresa = empresasResponse.Data?.FirstOrDefault(e => e.IssueTypeId == tipoMaterial.Value);
                }
                else
                {
                    empresa = empresasResponse.Data?.FirstOrDefault();
                }

                List<MaterialIa> materialesFactura = new List<MaterialIa>();
                if (nombresMateriales != null && nombresMateriales.Count > 0)
                {
                    materialesFactura = materialesResponse.Data?
                        .Where(m => nombresMateriales.Contains(m.Name, System.StringComparer.OrdinalIgnoreCase))
                        .ToList() ?? new List<MaterialIa>();
                }
                else if (tipoMaterial.HasValue && masBarato)
                {
                    var materialMasBarato = materialesResponse.Data?
                        .Where(m => m.Type == tipoMaterial.Value)
                        .OrderBy(m => m.Cost)
                        .FirstOrDefault();
                    if (materialMasBarato != null)
                        materialesFactura = new List<MaterialIa> { materialMasBarato };
                }
                else if (tipoMaterial.HasValue && masCaro)
                {
                    var materialMasCaro = materialesResponse.Data?
                        .Where(m => m.Type == tipoMaterial.Value)
                        .OrderByDescending(m => m.Cost)
                        .FirstOrDefault();
                    if (materialMasCaro != null)
                        materialesFactura = new List<MaterialIa> { materialMasCaro };
                }
                else if (tipoMaterial.HasValue)
                {
                    materialesFactura = materialesResponse.Data?
                        .Where(m => m.Type == tipoMaterial.Value)
                        .ToList() ?? new List<MaterialIa>();
                }
                else
                {
                    materialesFactura = materialesResponse.Data ?? new List<MaterialIa>();
                }

                if (empresa == null)
                {
                    return new PluginRespuesta { Success = false, Error = "No se encontró ninguna empresa con ese nombre o tipo." };
                }
                if (materialesFactura.Count == 0)
                {
                    return new PluginRespuesta { Success = false, Error = "No se encontraron materiales con esos criterios." };
                }

                // Conversación + opción de factura
                decimal iva = 0.21m;
                decimal costeEmpresa = empresa.Price;
                decimal costeMateriales = materialesFactura.Sum(m => m.Cost);
                decimal ivaEmpresa = costeEmpresa * iva;
                decimal ivaMateriales = costeMateriales * iva;
                decimal totalIva = ivaEmpresa + ivaMateriales;
                decimal total = costeEmpresa + costeMateriales + totalIva;

                var sb = new System.Text.StringBuilder();
                sb.AppendLine($"Empresa: {empresa.Name} - Coste: {costeEmpresa:F2}€");
                foreach (var m in materialesFactura)
                {
                    sb.AppendLine($"Material: {m.Name} - Coste: {m.Cost:F2}€");
                }
                sb.AppendLine($"IVA: {totalIva:F2}€");
                sb.AppendLine($"TOTAL: {total:F2}€");

                sb.AppendLine();
                sb.AppendLine("¿Quieres que te genere la factura con estos datos? Si es así, responde 'sí, genera la factura'. Si necesitas cambiar algo, dime qué quieres modificar.");

                return new PluginRespuesta { Success = true, Data = sb.ToString() };
            }

            if (intencion == IntencionUsuario.Salir)
            {
                return new PluginRespuesta
                {
                    Success = true,
                    Error = null,
                    Data = "👋 Hasta luego. Gracias por usar CleanFixBot."
                };
            }

            return new PluginRespuesta
            {
                Success = false,
                Error = "🤖 No entendí tu mensaje. Prueba con 'factura', 'materiales' o 'empresas'.",
                Data = null
            };
        }
    }
}
