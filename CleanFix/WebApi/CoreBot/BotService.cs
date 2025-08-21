using System.Threading.Tasks;
using CleanFix.Plugins;

namespace WebApi.CoreBot
{
    public class BotService : IBotService
    {
        public async Task<PluginRespuesta> ProcesarMensajeAsync(string mensaje)
        {
            var respuesta = await SimularRespuestaAsync(mensaje);

            return new PluginRespuesta
            {
                Success = true,
                Error = null,
                Data = new { Mensaje = respuesta }
            };
        }

        private Task<string> SimularRespuestaAsync(string mensaje)
        {
            return Task.FromResult($"🤖 Bot responde: '{mensaje}'");
        }
    }
}
