using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CleanFix.Plugins;
using WebApi.Models;

namespace WebApi.Services
{
    public class RecomendacionService
    {
        public string RecomendarAlternativas(List<CompanyIa> empresas, List<MaterialIa> materiales, string empresaActual, List<string> materialesActuales)
        {
            var alternativas = new System.Text.StringBuilder();
            var otrasEmpresas = empresas.Where(e => e.Name != empresaActual).Take(2).ToList();
            var otrosMateriales = materiales.Where(m => !materialesActuales.Contains(m.Name)).Take(2).ToList();
            if (otrasEmpresas.Any())
            {
                alternativas.AppendLine("Empresas alternativas:");
                foreach (var e in otrasEmpresas)
                    alternativas.AppendLine($"- {e.Name} (Precio: {e.Price:F2}€)");
            }
            if (otrosMateriales.Any())
            {
                alternativas.AppendLine("Materiales alternativos:");
                foreach (var m in otrosMateriales)
                    alternativas.AppendLine($"- {m.Name} (Coste: {m.Cost:F2}€)");
            }
            return alternativas.ToString();
        }
    }
}
