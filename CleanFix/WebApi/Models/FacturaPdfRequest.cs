using System.Collections.Generic;

namespace WebApi.Models
{
    public class FacturaPdfRequest
    {
        public string EmpresaNombre { get; set; }
        public List<string> MaterialesNombres { get; set; }
        public string EmailDestino { get; set; } // Para envío por email
    }
}
