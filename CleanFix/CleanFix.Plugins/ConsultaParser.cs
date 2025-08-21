using System.Text.RegularExpressions;

namespace CleanFix.Plugins
{
    public static class ConsultaParser
    {
        public static int? ExtraerTipo(string input, string entidad)
        {
            // Busca patrones como "empresa tipo 2" o "material del tipo 3"
            var match = Regex.Match(input, $@"{entidad}\s*(?:del\s*tipo|de\s*tipo|tipo)?\s*(\d+)", RegexOptions.IgnoreCase);
            if (match.Success && int.TryParse(match.Groups[1].Value, out int tipo))
                return tipo;
            return null;
        }

        public static bool SolicitaMasBarato(string input)
        {
            return Regex.IsMatch(input, @"más barato|mas barato", RegexOptions.IgnoreCase);
        }

        public static bool SolicitaMasCaro(string input)
        {
            return Regex.IsMatch(input, @"más caro|mas caro|más cara|mas cara", RegexOptions.IgnoreCase);
        }

        public static bool SolicitaDisponibles(string input)
        {
            return Regex.IsMatch(input, @"disponibles|todo lo disponible", RegexOptions.IgnoreCase);
        }
    }
}
