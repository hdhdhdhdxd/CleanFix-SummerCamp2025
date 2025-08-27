using CleanFix.Plugins;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class FacturaRequest
    {
        public CompanyIa Empresa { get; set; }
        public List<MaterialIa> Materiales { get; set; }
    }
}
