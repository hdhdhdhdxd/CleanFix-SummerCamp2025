using CleanFix.Plugins;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Collections.Generic;

namespace WebApi.CoreBot
{
    /// <summary>
    /// Servicio principal del bot CleanFixBot. Gestiona la interpretación de mensajes y la delegación a plugins.
    /// </summary>
    public class CleanFixBotService : IBotService
    {
        private readonly Dictionary<string, IPlugin> _plugins;
        private readonly IClasificadorIntencion _clasificador;

        public CleanFixBotService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("CleanFixDB");

            _plugins = new Dictionary<string, IPlugin>
            {
                { "factura", new FacturaPluginTestPG() },
                { "db", new DBPluginTestPG(connectionString) }
            };

            _clasificador = new ClasificadorIntencion();
        }

        /// <summary>
        /// Procesa el mensaje recibido, interpreta la intención y delega la consulta al plugin adecuado.
        /// </summary>
        public async Task<PluginRespuesta> ProcesarMensajeAsync(string mensaje)
        {
            var intencion = _clasificador.Clasificar(mensaje);

            // Si la intención es consultar datos, parsea la consulta natural
            if (intencion == IntencionUsuario.ConsultarDatos)
            {
                int? tipoEmpresa = ConsultaParser.ExtraerTipo(mensaje, "empresa");
                int? tipoMaterial = ConsultaParser.ExtraerTipo(mensaje, "material");
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

            // Si la intención es generar factura, busca los datos y genera la factura
            if (intencion == IntencionUsuario.GenerarFactura)
            {
                // Buscar empresa según criterio
                var empresasResponse = await _plugins["db"].EjecutarAsync(mensaje.Contains("empresa") ? mensaje : "empresas");
                var empresas = empresasResponse.Data as List<CompanyIa>;
                CompanyIa empresa = empresas?.FirstOrDefault();

                // Buscar materiales según criterio
                var materialesResponse = await _plugins["db"].EjecutarAsync(mensaje.Contains("material") ? mensaje : "materiales");
                var materiales = materialesResponse.Data as List<MaterialIa>;
                var materialesFactura = materiales ?? new List<MaterialIa>();

                if (empresa == null)
                {
                    return new PluginRespuesta { Success = false, Error = "No se encontró ninguna empresa que cumpla el criterio." };
                }

                var facturaPlugin = new FacturaPluginTestPG();
                string factura = facturaPlugin.GenerarFactura(empresa, materialesFactura);
                return new PluginRespuesta { Success = true, Data = factura };
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
