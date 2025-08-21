using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanFix.Plugins;
using Microsoft.SemanticKernel;

namespace CleanFix.Plugins
{
    public class FacturaPluginTestPG : IPlugin
    {
        private const decimal IVA = 0.21m;

        [KernelFunction]
        public string GenerarFactura(CompanyIa empresa, List<MaterialIa> materialesIa)
        {
            decimal costoEmpresa = empresa.Price;
            decimal ivaEmpresa = costoEmpresa * IVA;

            decimal costoMateriales = materialesIa.Sum(m => m.Cost);
            decimal ivaMateriales = costoMateriales * IVA;

            decimal total = costoEmpresa + ivaEmpresa + costoMateriales + ivaMateriales;

            StringBuilder sb = new();
            sb.AppendLine();
            sb.AppendLine(" Empresa proveedora:");
            sb.AppendLine($" - Nombre: {empresa.Name}");
            sb.AppendLine();
            sb.AppendLine(" Materiales incluidos:");
            sb.AppendLine(" | ID | Nombre             | Costo    | IVA (21%) | Total    |");
            sb.AppendLine(" |----|--------------------|----------|-----------|----------|");

            foreach (var m in materialesIa)
            {
                decimal ivaMat = m.Cost * IVA;
                decimal totalMat = m.Cost + ivaMat;
                sb.AppendLine($" | {m.Id}  | {m.Name}    | €{m.Cost:F2} | €{ivaMat:F2}    | €{totalMat:F2} |");
            }

            sb.AppendLine();
            sb.AppendLine(" -----------------------------------------");
            sb.AppendLine($" Total materiales: €{costoMateriales:F2}");
            sb.AppendLine($" IVA materiales (21%): €{ivaMateriales:F2}");
            sb.AppendLine($" Costo empresa: €{costoEmpresa:F2}");
            sb.AppendLine($" IVA empresa (21%): €{ivaEmpresa:F2}");
            sb.AppendLine($" *TOTAL FACTURA:*   *€{total:F2}*");
            sb.AppendLine(" -----------------------------------------");

            return sb.ToString();
        }

        [KernelFunction]
        public string ObtenerIVA(CompanyIa empresa, List<MaterialIa> materialesIa)
        {
            decimal ivaEmpresa = empresa.Price * IVA;
            decimal ivaMateriales = materialesIa.Sum(m => m.Cost) * IVA;
            decimal ivaTotal = ivaEmpresa + ivaMateriales;

            return $" IVA empresa: €{ivaEmpresa:F2}\n IVA materiales: €{ivaMateriales:F2}\n IVA total: €{ivaTotal:F2}";
        }

        // ✅ Método requerido por la interfaz IPlugin
        public Task<string> EjecutarAsync(string mensaje)
        {
            // Simulación de datos de prueba
            var empresa = new CompanyIa { Name = "Empresa Test", Price = 1000 };
            var materiales = new List<MaterialIa>
            {
                new MaterialIa { Id = 1, Name = "Material A", Cost = 200 },
                new MaterialIa { Id = 2, Name = "Material B", Cost = 300 }
            };

            // Lógica básica para decidir qué función ejecutar
            if (mensaje.Contains("IVA", StringComparison.OrdinalIgnoreCase))
            {
                var resultado = ObtenerIVA(empresa, materiales);
                return Task.FromResult(resultado);
            }
            else
            {
                var resultado = GenerarFactura(empresa, materiales);
                return Task.FromResult(resultado);
            }
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
