using System.IO;
using System.Threading.Tasks;
using WebApi.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Linq;

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
            var materiales = factura.Materiales.Select(m => new
            {
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
                    page.Header().Text($"Factura CleanFixBot").SemiBold().FontSize(20);
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Datos de la factura").Bold().FontSize(14);
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3); // Concepto
                                columns.RelativeColumn(2); // Precio sin IVA
                                columns.RelativeColumn(2); // IVA
                                columns.RelativeColumn(2); // Precio con IVA
                            });
                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Concepto").Bold();
                                header.Cell().Element(CellStyle).Text("Precio sin IVA").Bold();
                                header.Cell().Element(CellStyle).Text("IVA").Bold();
                                header.Cell().Element(CellStyle).Text("Precio con IVA").Bold();
                            });
                            // Empresa/servicio
                            table.Cell().Element(CellStyle).Text($"Servicio: {factura.Empresa.Nombre}");
                            table.Cell().Element(CellStyle).Text($"{costeEmpresa:F2} €");
                            table.Cell().Element(CellStyle).Text($"{ivaEmpresa:F2} €");
                            table.Cell().Element(CellStyle).Text($"{totalEmpresa:F2} €");
                            // Materiales
                            foreach (var mat in materiales)
                            {
                                table.Cell().Element(CellStyle).Text($"Material: {mat.Nombre}");
                                table.Cell().Element(CellStyle).Text($"{mat.Coste:F2} €");
                                table.Cell().Element(CellStyle).Text($"{mat.IVA:F2} €");
                                table.Cell().Element(CellStyle).Text($"{mat.Total:F2} €");
                            }
                        });
                        col.Item().PaddingVertical(10);
                        col.Item().Text($"Resumen").Bold().FontSize(14);
                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(2);
                            });
                            table.Cell().Element(CellStyle).Text("Total sin IVA:");
                            table.Cell().Element(CellStyle).Text($"{totalSinIva:F2} €");
                            table.Cell().Element(CellStyle).Text("IVA total:");
                            table.Cell().Element(CellStyle).Text($"{totalIva:F2} €");
                            table.Cell().Element(CellStyle).Text("TOTAL CON IVA:").Bold();
                            table.Cell().Element(CellStyle).Text($"{totalConIva:F2} €").Bold();
                        });
                    });
                });
            });
            static IContainer CellStyle(IContainer container) => container.PaddingVertical(2).PaddingHorizontal(4);
            using var ms = new MemoryStream();
            document.GeneratePdf(ms);
            return ms.ToArray();
        }
    }
}
