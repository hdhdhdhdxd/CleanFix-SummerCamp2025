using System.IO;
using System.Threading.Tasks;
using WebApi.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;
using System.Text;

namespace WebApi.Services
{
    public class FacturaPdfService : IFacturaPdfService
    {
        public async Task<byte[]> GenerarFacturaPdfAsync(FacturaDetalleDto factura)
        {
            decimal iva = 0.21m;
            decimal costeEmpresa = factura.Empresa.Coste;
            decimal ivaEmpresa = costeEmpresa * iva;
            decimal totalEmpresa = costeEmpresa + ivaEmpresa;
            var materiales = factura.Materiales.Select((m, idx) => new
            {
                Id = idx + 1,
                Nombre = m.Nombre,
                Coste = m.Coste,
                IVA = m.Coste * iva,
                Total = m.Coste * (1 + iva)
            }).ToList();
            decimal totalMaterialesSinIva = materiales.Sum(m => m.Coste);
            decimal totalMaterialesIva = materiales.Sum(m => m.IVA);
            decimal totalMaterialesConIva = materiales.Sum(m => m.Total);
            decimal totalSinIva = costeEmpresa + totalMaterialesSinIva;
            decimal totalIva = ivaEmpresa + totalMaterialesIva;
            decimal totalConIva = totalSinIva + totalIva;

            // Construir el string de la "tabla" con separadores
            var sb = new StringBuilder();
            sb.AppendLine(" Empresa proveedora:");
            sb.AppendLine();
            sb.AppendLine($" - Nombre: {factura.Empresa.Nombre}");
            sb.AppendLine();
            sb.AppendLine(" Materiales incluidos:");
            sb.AppendLine();
            sb.AppendLine(" | ID | Nombre             | Costo    | IVA (21%) | Total    |");
            sb.AppendLine(" |----|--------------------|----------|-----------|----------|");
            foreach (var m in materiales)
            {
                sb.AppendLine($" | {m.Id,2} | {m.Nombre,-18} | €{m.Coste,7:F2} | €{m.IVA,8:F2} | €{m.Total,7:F2} |");
            }
            sb.AppendLine();
            sb.AppendLine(" -----------------------------------------");
            sb.AppendLine($" Total materiales: €{totalMaterialesSinIva:F2}");
            sb.AppendLine($" IVA materiales (21%): €{totalMaterialesIva:F2}");
            sb.AppendLine(" -----------------------------------------");
            sb.AppendLine($" Costo empresa: €{costeEmpresa:F2}");
            sb.AppendLine($" IVA empresa (21%): €{ivaEmpresa:F2}");
            sb.AppendLine(" -----------------------------------------");
            sb.AppendLine($" *TOTAL FACTURA:*");
            sb.AppendLine($"   *€{totalConIva:F2}*");
            sb.AppendLine(" -----------------------------------------");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text($"Factura CleanFixBot").SemiBold().FontSize(20);
                    page.Content().Text(sb.ToString()).FontFamily("Consolas").FontSize(11);
                });
            });
            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            return ms.ToArray();
        }
    }
}
