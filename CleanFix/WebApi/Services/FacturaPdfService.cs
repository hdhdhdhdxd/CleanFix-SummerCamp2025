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

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Content().Column(col =>
                    {
                        col.Item().Text("Factura CleanFix").Bold().FontSize(28).AlignCenter();
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text("Empresa proveedora:").Bold().FontSize(13);
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text($" - Nombre: {factura.Empresa.Nombre}").FontSize(11);
                        col.Item().Text("").FontSize(6);
                        col.Item().Text("Materiales incluidos:").Bold().FontSize(13);
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text("").FontSize(6); // Espacio
                        // Tabla simulada con monoespaciado
                        var sb = new StringBuilder();
                        sb.AppendLine(" | ID | Nombre             | Costo    | IVA (21%) | Total    |");
                        sb.AppendLine(" |----|--------------------|----------|-----------|----------|");
                        foreach (var m in materiales)
                        {
                            sb.AppendLine($" | {m.Id,2} | {m.Nombre,-18} | €{m.Coste,7:F2} | €{m.IVA,8:F2} | €{m.Total,7:F2} |");
                        }
                        sb.AppendLine(" |----|--------------------|----------|-----------|----------|");
                        col.Item().Text(sb.ToString()).FontFamily("Consolas").FontSize(11);
                        col.Item().Text("").FontSize(6);
                        col.Item().Text("").FontSize(6);
                        col.Item().Text(" -----------------------------------------");
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text($" Total materiales: €{totalMaterialesSinIva:F2}");
                        col.Item().Text($" IVA materiales (21%): €{totalMaterialesIva:F2}");
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text(" -----------------------------------------");
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text($" Costo empresa: €{costeEmpresa:F2}");
                        col.Item().Text($" IVA empresa (21%): €{ivaEmpresa:F2}");
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text(" -----------------------------------------");
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text("*TOTAL FACTURA:*").Bold().FontSize(13);
                        col.Item().Text($"   *€{totalConIva:F2}*").Bold().FontSize(13);
                        col.Item().Text("").FontSize(6); // Espacio
                        col.Item().Text(" -----------------------------------------");
                    });
                });
            });
            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            return ms.ToArray();
        }
    }
}
