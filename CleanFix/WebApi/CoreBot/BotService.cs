using System.Threading.Tasks;
using CleanFix.Plugins;

namespace WebApi.CoreBot
{
    public class BotService : IBotService
    {
        private readonly IPlugin _plugin;
        public BotService(IPlugin plugin)
        {
            _plugin = plugin;
        }
        public async Task<PluginRespuesta> ProcesarMensajeAsync(string mensaje)
        {
            return await _plugin.EjecutarAsync(mensaje);
        }
    }
}
