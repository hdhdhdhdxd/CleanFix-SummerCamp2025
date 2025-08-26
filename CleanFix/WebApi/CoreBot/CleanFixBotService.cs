using CleanFix.Plugins;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

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

            // Si la intención es generar factura, delega al plugin de factura
            if (intencion == IntencionUsuario.GenerarFactura)
            {
                return await _plugins["factura"].EjecutarAsync(mensaje);
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
