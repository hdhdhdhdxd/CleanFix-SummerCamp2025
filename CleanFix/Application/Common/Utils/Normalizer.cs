using System.Text.RegularExpressions;

namespace Application.Common.Utils
{
    public static class Normalizer
    {
        public static string NormalizarNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return nombre;
            nombre = nombre.Trim();
            return Regex.Replace(nombre, @"\s+", " ");
        }
    }
}
