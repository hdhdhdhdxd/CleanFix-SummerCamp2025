using System.IO;
using System.Threading.Tasks;
using WebApi.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

namespace WebApi.Services
{
    public class FacturaPdfService : IFacturaPdfService
    {
        public async Task<byte[]> GenerarFacturaPdfAsync(FacturaDetalleDto factura)
        {
            // Generación simple de PDF con QuestPDF
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Text($"Factura CleanFixBot").SemiBold().FontSize(20);
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Empresa: {factura.Empresa.Nombre} - Coste: {factura.Empresa.Coste:F2}€");
                        foreach (var mat in factura.Materiales)
                        {
                            col.Item().Text($"Material: {mat.Nombre} - Coste: {mat.Coste:F2}€");
                        }
                        col.Item().Text($"TOTAL CON IVA: {factura.TotalConIVA:F2}€").Bold();
                    });
                });
            });
            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            return ms.ToArray();
        }
    }
}
