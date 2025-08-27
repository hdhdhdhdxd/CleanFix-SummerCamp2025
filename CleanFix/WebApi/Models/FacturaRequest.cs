using CleanFix.Plugins;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class FacturaRequest
    {
        public string EmpresaNombre { get; set; }
        public List<string> MaterialesNombres { get; set; }
    }
}
