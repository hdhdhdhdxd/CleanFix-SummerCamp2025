using System.Collections.Generic;
using System.Threading.Tasks;
using CleanFix.Plugins;

namespace WebApi.CoreBot
{
    public class CleanFixBotService : IBotService
    {
        private readonly Dictionary<string, IPlugin> _plugins;

        public CleanFixBotService(IConfiguration config)
        {
            var connectionString = config.GetConnectionString("CleanFixDB");

            _plugins = new Dictionary<string, IPlugin>
        {
            { "factura", new FacturaPluginTestPG() },
            { "db", new DBPluginTestPG(connectionString) }
        };
        }

        public async Task<string> ProcesarMensajeAsync(string mensaje)
        {
            var intencion = ClasificarIntencion(mensaje);
            if (_plugins.TryGetValue(intencion, out var plugin))
            {
                return await plugin.EjecutarAsync(mensaje);
            }

            return "No entendí tu mensaje 🤖";
        }

        private string ClasificarIntencion(string mensaje)
        {
            mensaje = mensaje.ToLower();

            if (mensaje.Contains("factura")) return "factura";
            if (mensaje.Contains("base de datos") || mensaje.Contains("empresa") || mensaje.Contains("material") || mensaje.Contains("materiales") || mensaje.Contains("empresas")) return "db";

            return "desconocido";
        }
    }
}
