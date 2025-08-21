using CleanFix.Plugins;

namespace WebApi.CoreBot
{
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

            _clasificador = new ClasificadorIntencion(); // ✅ instanciamos el clasificador
        }

        public async Task<PluginRespuesta> ProcesarMensajeAsync(string mensaje)
        {
            var intencion = _clasificador.Clasificar(mensaje);

            switch (intencion)
            {
                case IntencionUsuario.GenerarFactura:
                    return await _plugins["factura"].EjecutarAsync(mensaje);

                case IntencionUsuario.ConsultarDatos:
                    return await _plugins["db"].EjecutarAsync(mensaje);

                case IntencionUsuario.Salir:
                    return new PluginRespuesta
                    {
                        Success = true,
                        Error = null,
                        Data = "👋 Hasta luego. Gracias por usar CleanFixBot."
                    };

                default:
                    return new PluginRespuesta
                    {
                        Success = false,
                        Error = "🤖 No entendí tu mensaje. Prueba con 'factura', 'materiales' o 'empresas'.",
                        Data = null
                    };
            }
        }
    }
}
