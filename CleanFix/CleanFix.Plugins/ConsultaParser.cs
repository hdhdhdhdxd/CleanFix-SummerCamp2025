using System.Text.RegularExpressions;
using Microsoft.SemanticKernel;
using System.Text.Json;
using System.Threading.Tasks;

namespace CleanFix.Plugins
{
    public class EmpresaFiltro
    {
        public int? Tipo { get; set; }
        public decimal? PrecioMin { get; set; }
        public bool MasBarata { get; set; }
        public bool MasCara { get; set; }
    }

    public static class SemanticEmpresaHelper
    {
        private static readonly string prompt = @"Extrae los filtros de la siguiente consulta de usuario sobre empresas:\nUsuario: {{$input}}\nDevuelve un JSON con los campos: tipo (número o null), precioMin (número o null), masBarata (true/false), masCara (true/false).";

        public static async Task<EmpresaFiltro> ExtraerFiltrosAsync(string mensaje, Kernel kernel)
        {
            var function = kernel.CreateFunctionFromPrompt(prompt);
            var result = await function.InvokeAsync(kernel, new() { ["input"] = mensaje });
            return JsonSerializer.Deserialize<EmpresaFiltro>(result.GetValue<string>() ?? "{}");
        }
    }

    public static class ConsultaParser
    {
        public static int? ExtraerTipo(string input, string entidad)
        {
            // Busca patrones como "empresa tipo 2", "empresa tipo=2", "material del tipo 3", etc.
            var match = Regex.Match(input, $@"{entidad}\s*(?:del\s*tipo|de\s*tipo|tipo)?\s*[=:]?\s*(\d+)", RegexOptions.IgnoreCase);
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
