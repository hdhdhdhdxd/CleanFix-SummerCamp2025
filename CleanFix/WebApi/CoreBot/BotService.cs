using System.Threading.Tasks;
using CleanFix.Plugins;

namespace WebApi.CoreBot
{
    /// <summary>
    /// Servicio base para el bot CleanFixBot. Encapsula la ejecución de un plugin.
    /// </summary>
    public class BotService : IBotService
    {
        private readonly IPlugin _plugin;
        public BotService(IPlugin plugin)
        {
            _plugin = plugin;
        }
        /// <summary>
        /// Procesa el mensaje recibido y lo delega al plugin configurado.
        /// </summary>
        public async Task<PluginRespuesta> ProcesarMensajeAsync(string mensaje)
        {
            return await _plugin.EjecutarAsync(mensaje);
        }
    }
}
