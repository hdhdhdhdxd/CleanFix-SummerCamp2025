using System.Text.RegularExpressions;
using Microsoft.SemanticKernel;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;

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
            return JsonSerializer.Deserialize<EmpresaFiltro>(result.GetValue<string>() ?? "{}" );
        }
    }

    /// <summary>
    /// Utilidades para analizar y extraer intenciones y parámetros de las consultas del usuario.
    /// </summary>
    public static class ConsultaParser
    {
        /// <summary>
        /// Extrae el tipo (número) de empresa o material desde el texto del usuario.
        /// </summary>
        public static int? ExtraerTipo(string input, string entidad)
        {
            var match = Regex.Match(input, $@"{entidad}\s*(?:del\s*tipo|de\s*tipo|tipo)?\s*(\d+)", RegexOptions.IgnoreCase);
            if (match.Success && int.TryParse(match.Groups[1].Value, out int tipo))
                return tipo;
            return null;
        }

        /// <summary>
        /// Extrae el IssueTypeId a partir del nombre del problema (por ejemplo, "eléctrico").
        /// </summary>
        public static int? ExtraerIssueTypeIdPorNombre(string mensaje, List<IssueType> issueTypes)
        {
            foreach (var issue in issueTypes)
            {
                if (mensaje.Contains(issue.Name, StringComparison.OrdinalIgnoreCase))
                    return issue.Id;
            }
            return null;
        }

        /// <summary>
        /// Extrae el nombre de la empresa desde el texto del usuario.
        /// </summary>
        public static string ExtraerNombreEmpresa(string input)
        {
            var match = Regex.Match(input, @"empresa\s+(?:llamada|con nombre|que se llama)?\s*([\wáéíóúüñÁÉÍÓÚÜÑ\s]+)", RegexOptions.IgnoreCase);
            if (match.Success)
                return match.Groups[1].Value.Trim();
            return null;
        }

        /// <summary>
        /// Extrae los nombres de materiales desde el texto del usuario.
        /// </summary>
        public static List<string> ExtraerNombresMateriales(string input)
        {
            var match = Regex.Match(input, @"material(?:es)?\s+(?:llamados?|con nombre|que se llaman)?\s*([\wáéíóúüñÁÉÍÓÚÜÑ\s,]+)", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                var nombres = match.Groups[1].Value.Split(',').Select(n => n.Trim()).Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
                return nombres;
            }
            return new List<string>();
        }

        /// <summary>
        /// Detecta si el usuario solicita el material más barato.
        /// </summary>
        public static bool SolicitaMasBarato(string input)
        {
            var patrones = new[]
            {
                @"material\s+(de\s+tipo|del\s+tipo|tipo)?\s*\d+\s+(más barato|mas barato)",
                @"el\s+material\s+(más barato|mas barato)",
                @"producto\s+(más barato|mas barato)",
                @"insumo\s+(más barato|mas barato)",
                @"el\s+(más barato|mas barato)"
            };
            return patrones.Any(p => Regex.IsMatch(input, p, RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Detecta si el usuario solicita el material más caro.
        /// </summary>
        public static bool SolicitaMasCaro(string input)
        {
            var patrones = new[]
            {
                @"material\s+(de\s+tipo|del\s+tipo|tipo)?\s*\d+\s+(más caro|mas caro|más cara|mas cara)",
                @"el\s+material\s+(más caro|mas caro|más cara|mas cara)",
                @"producto\s+(más caro|mas caro|más cara|mas cara)",
                @"insumo\s+(más caro|mas caro|más cara|mas cara)",
                @"el\s+(más caro|mas caro|más cara|mas cara)"
            };
            return patrones.Any(p => Regex.IsMatch(input, p, RegexOptions.IgnoreCase));
        }

        /// <summary>
        /// Detecta si el usuario solicita todos los materiales disponibles.
        /// </summary>
        public static bool SolicitaTodosMateriales(string input)
        {
            var frases = new[]
            {
                "todos los materiales",
                "materiales disponibles",
                "todo lo que haya",
                "todos los productos",
                "todo lo disponible"
            };
            return frases.Any(f => input.Contains(f, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Detecta si el usuario solicita materiales disponibles.
        /// </summary>
        public static bool SolicitaDisponibles(string input)
        {
            return input.Contains("disponibles", StringComparison.OrdinalIgnoreCase);
        }
    }
}
