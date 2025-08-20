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

            // Simula el estilo conversacional que usabas en consola
            return intencion switch
            {
                IntencionUsuario.GenerarFactura => "Bot: Claro, puedo ayudarte a generar una factura. ¿Tienes el número de pedido?",
                IntencionUsuario.ConsultarDatos => "Bot: ¿Qué datos necesitas consultar? Puedo ayudarte con información de empresa o materiales.",
                IntencionUsuario.Salir => "Bot: Ha sido un placer ayudarte. ¡Hasta pronto!",
                _ => "Bot: Lo siento, no entendí tu solicitud. ¿Puedes explicarlo de otra forma?"
            };
        }
    }
