using System.Threading.Tasks;

namespace CleanFix.Plugins
{
    public interface IPlugin
    {
        Task<PluginRespuesta> EjecutarAsync(string mensaje);
    }
}
