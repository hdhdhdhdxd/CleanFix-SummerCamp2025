using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Domain.Entities;
using Microsoft.SemanticKernel;

public class FacturaPluginTest
{
    private const decimal IVA = 0.21m;

    [KernelFunction]
    public string GenerarFactura(Company empresa, List<Material> materiales)
    {
        decimal costoEmpresa = empresa.Price;
        decimal ivaEmpresa = costoEmpresa * IVA;

        decimal costoMateriales = materiales.Sum(m => m.Cost);
        decimal ivaMateriales = costoMateriales * IVA;

        decimal total = costoEmpresa + ivaEmpresa + costoMateriales + ivaMateriales;

        StringBuilder sb = new();
        sb.AppendLine("---");
        sb.AppendLine("## **Factura**");
        sb.AppendLine();
        sb.AppendLine("**Empresa proveedora:**");
        sb.AppendLine($"- **Nombre:** {empresa.Name}");
        sb.AppendLine($"- **Email:** {empresa.Email}");
        sb.AppendLine($"- **Precio de servicio:** ${costoEmpresa:F2}");
        sb.AppendLine($"- **IVA servicio (21%):** ${ivaEmpresa:F2}");
        sb.AppendLine();
        sb.AppendLine("**Materiales incluidos:**");
        sb.AppendLine();
        sb.AppendLine("| ID | Nombre             | Costo    | IVA (21%) | Total    |");
        sb.AppendLine("|----|--------------------|----------|-----------|----------|");

        foreach (var m in materiales)
        {
            decimal ivaMat = m.Cost * IVA;
            decimal totalMat = m.Cost + ivaMat;
            sb.AppendLine($"| {m.Id}  | {m.Name}    | ${m.Cost:F2} | ${ivaMat:F2}    | ${totalMat:F2} |");
        }

        sb.AppendLine();
        sb.AppendLine($"**Total materiales:** ${costoMateriales:F2}");
        sb.AppendLine($"**IVA materiales (21%):** ${ivaMateriales:F2}");
        sb.AppendLine();
        sb.AppendLine($"**Costo empresa:** ${costoEmpresa:F2}");
        sb.AppendLine($"**IVA empresa (21%):** ${ivaEmpresa:F2}");
        sb.AppendLine();
        sb.AppendLine("---");
        sb.AppendLine();
        sb.AppendLine($"### **TOTAL FACTURA:**");
        sb.AppendLine($"**${total:F2}**");
        sb.AppendLine();
        sb.AppendLine("---");

        return sb.ToString();
    }

    [KernelFunction]
    public string ObtenerIVA(Company empresa, List<Material> materiales)
    {
        decimal ivaEmpresa = empresa.Price * IVA;
        decimal ivaMateriales = materiales.Sum(m => m.Cost) * IVA;
        decimal ivaTotal = ivaEmpresa + ivaMateriales;

        return $"IVA empresa: ${ivaEmpresa:F2}\nIVA materiales: ${ivaMateriales:F2}\nIVA total: ${ivaTotal:F2}";
    }
}
public class Factura
{
    public string EmpresaNombre { get; set; }
    public string Direccion { get; set; }
    public string Telefono { get; set; }

    public List<Material> Materiales { get; set; }

    public decimal Subtotal { get; set; }
    public decimal Impuestos { get; set; }
    public decimal Total { get; set; }

    public DateTime Fecha { get; set; }
}
