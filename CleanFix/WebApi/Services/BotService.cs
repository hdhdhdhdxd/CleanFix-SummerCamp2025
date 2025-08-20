using CleanFixConsola.PluginsIATest;

namespace WebApi.Servicios
{
    public class BotService
    {
        private readonly IClasificadorIntencion _clasificador;

        public BotService(IClasificadorIntencion clasificador)
        {
            _clasificador = clasificador;
        }

        public string ProcesarMensaje(string mensaje)
        {
            var intencion = _clasificador.Clasificar(mensaje);

            return intencion switch
            {
                IntencionUsuario.GenerarFactura => "Entendido: quieres generar una factura.",
                IntencionUsuario.ConsultarDatos => "Vamos a consultar los datos que necesitas.",
                IntencionUsuario.Salir => "Hasta luego. ¡Gracias por usar el servicio!",
                _ => "No he podido entender tu intención. ¿Puedes reformular?"
            };
        }
    }
}
