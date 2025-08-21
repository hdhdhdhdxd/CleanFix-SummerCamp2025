using System.Threading.Tasks;
using CleanFix.Plugins;
using WebApi.CoreBot.Models;

namespace WebApi.CoreBot
{
    public interface IBotService
    {
        Task<PluginRespuesta> ProcesarMensajeAsync(string mensaje);
    }
}
