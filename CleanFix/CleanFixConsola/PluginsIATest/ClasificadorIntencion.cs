using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanFixConsola.PluginsIATest
{
    public enum IntencionUsuario
    {
        GenerarFactura,
        ConsultarDatos,
        Salir,
        Desconocida
    }

    public interface IClasificadorIntencion
    {
        IntencionUsuario Clasificar(string input);
    }

    //Detección de la intención del usuario basada en el texto de entrada
    public class ClasificadorIntencion : IClasificadorIntencion
    {
        public IntencionUsuario Clasificar(string input)
        {
            input = input.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(input))
                return IntencionUsuario.Desconocida;

            if (input.Contains("salir"))
                return IntencionUsuario.Salir;

            if (input.Contains("factura") || input.Contains("crear") || input.Contains("generar") || input.Contains("pedido"))
                return IntencionUsuario.GenerarFactura;

            if (input.Contains("empresa") || input.Contains("material") || input.Contains("ver") || input.Contains("mostrar") || input.Contains("consultar"))
                return IntencionUsuario.ConsultarDatos;

            return IntencionUsuario.Desconocida;
        }
    }
}
