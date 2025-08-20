using System.ComponentModel;
using System.Text;
using System.Text.Json;
using Domain.Entities;
using Microsoft.SemanticKernel;
using System.Globalization;

namespace CleanFixConsola.PluginsIATest
{
    public class FacturaPluginTest
    {
        // Constante que representa el porcentaje de IVA aplicado (21%)
        private const decimal IVA = 0.21m;

        // Función expuesta al kernel que genera una factura detallada en formato texto
        [KernelFunction]
        public string GenerarFactura(CompanyIa empresa, List<MaterialIa> materialesIa)
        {
            // Calcula el coste del servicio de la empresa y su IVA
            decimal costoEmpresa = empresa.Price;
            decimal ivaEmpresa = costoEmpresa * IVA;

            // Calcula el coste total de los materiales y su IVA
            decimal costoMateriales = materialesIa.Sum(m => m.Cost);
            decimal ivaMateriales = costoMateriales * IVA;

            // Calcula el total general de la factura (empresa + materiales + IVA)
            decimal total = costoEmpresa + ivaEmpresa + costoMateriales + ivaMateriales;

            // Construye el contenido de la factura en formato Markdown
            StringBuilder sb = new();
            sb.AppendLine();
            // Información de la empresa proveedora
            sb.AppendLine(" Empresa proveedora:");
            sb.AppendLine();
            sb.AppendLine($" - Nombre: {empresa.Name}");
            sb.AppendLine();

            // Tabla con los materiales incluidos en la factura
            sb.AppendLine(" Materiales incluidos:");
            sb.AppendLine();
            sb.AppendLine(" | ID | Nombre             | Costo    | IVA (21%) | Total    |");
            sb.AppendLine(" |----|--------------------|----------|-----------|----------|");

            // Recorre cada material y calcula su IVA y total individual
            foreach (var m in materialesIa)
            {
                decimal ivaMat = m.Cost * IVA;
                decimal totalMat = m.Cost + ivaMat;
                sb.AppendLine($" | {m.Id}  | {m.Name}    | €{m.Cost:F2} | €{ivaMat:F2}    | €{totalMat:F2} |");
                sb.AppendLine();
            }

            // Resumen de costes y totales
            sb.AppendLine();
            sb.AppendLine(" -----------------------------------------");
            sb.AppendLine($" Total materiales: €{costoMateriales:F2}");
            sb.AppendLine($" IVA materiales (21%): €{ivaMateriales:F2}");
            sb.AppendLine(" -----------------------------------------");
            sb.AppendLine($" Costo empresa: €{costoEmpresa:F2}");
            sb.AppendLine($" IVA empresa (21%): €{ivaEmpresa:F2}");
            sb.AppendLine(" -----------------------------------------");
            sb.AppendLine($" *TOTAL FACTURA:*");
            sb.AppendLine($"   *€{total:F2}*");
            sb.AppendLine(" -----------------------------------------");

            // Devuelve la factura como cadena de texto
            return sb.ToString();
        }

        // Función expuesta al kernel que calcula y devuelve solo el desglose del IVA
        [KernelFunction]
        public string ObtenerIVA(CompanyIa empresa, List<MaterialIa> materialesIa)
        {
            // Calcula el IVA del servicio de la empresa
            decimal ivaEmpresa = empresa.Price * IVA;

            // Calcula el IVA total de los materiales
            decimal ivaMateriales = materialesIa.Sum(m => m.Cost) * IVA;

            // Suma ambos para obtener el IVA total de la factura
            decimal ivaTotal = ivaEmpresa + ivaMateriales;

            // Devuelve el desglose en formato texto
            return $" IVA empresa: €{ivaEmpresa:F2}\n IVA materiales: €{ivaMateriales:F2}\n IVA total: €{ivaTotal:F2}";
        }
    }

    //Clases de datos para la factura
    public class Factura
    {
        public string EmpresaNombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public List<MaterialIa> Materiales { get; set; }

        public decimal Subtotal { get; set; }
        public decimal Impuestos { get; set; }
        public decimal Total { get; set; }

        public DateTime Fecha { get; set; }
    }
}
