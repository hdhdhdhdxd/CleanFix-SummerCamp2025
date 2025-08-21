using System.Threading.Tasks;
using CleanFix.Plugins;

namespace WebApi.CoreBot
{
    public interface IBotService
    {
        Task<PluginRespuesta> ProcesarMensajeAsync(string mensaje);
    }
}
