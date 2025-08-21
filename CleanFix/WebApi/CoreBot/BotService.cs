using System.Threading.Tasks;

namespace WebApi.CoreBot
{
    public class BotService : IBotService
    {
        public async Task<string> ProcesarMensajeAsync(string mensaje)
        {
            // Aquí puedes integrar tu lógica real del bot
            var respuesta = await SimularRespuestaAsync(mensaje);
            return respuesta;
        }

        private Task<string> SimularRespuestaAsync(string mensaje)
        {
            // Simulación básica
            return Task.FromResult($"🤖 Bot responde: '{mensaje}'");
        }
    }
}
