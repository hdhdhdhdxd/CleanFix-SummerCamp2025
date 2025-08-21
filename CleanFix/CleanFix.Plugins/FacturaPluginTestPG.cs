using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CleanFix.Plugins;
using WebApi.CoreBot.Models;
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

            var sb = new StringBuilder();
            sb.AppendLine($"Empresa: {empresa.Name}");
            sb.AppendLine($"Total factura: €{total:F2}");
            return sb.ToString();
        }

        [KernelFunction]
        public string ObtenerIVA(CompanyIa empresa, List<MaterialIa> materialesIa)
        {
            decimal ivaEmpresa = empresa.Price * IVA;
            decimal ivaMateriales = materialesIa.Sum(m => m.Cost) * IVA;
            decimal ivaTotal = ivaEmpresa + ivaMateriales;

            return $"IVA total: €{ivaTotal:F2}";
        }

        public async Task<PluginRespuesta> EjecutarAsync(string mensaje)
        {
            var empresa = new CompanyIa { Name = "Empresa Test", Price = 1000 };
            var materiales = new List<MaterialIa>
        {
            new MaterialIa { Id = 1, Name = "Material A", Cost = 200 },
            new MaterialIa { Id = 2, Name = "Material B", Cost = 300 }
            };

            string resultado = mensaje.Contains("iva", StringComparison.OrdinalIgnoreCase)
                ? ObtenerIVA(empresa, materiales)
                : GenerarFactura(empresa, materiales);

            return await Task.FromResult(new PluginRespuesta
            {
                Success = true,
                Error = null,
                Data = resultado
            });
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

