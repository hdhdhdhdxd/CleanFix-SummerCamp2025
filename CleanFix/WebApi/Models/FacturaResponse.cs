namespace WebApi.Models
{
    public class FacturaEmpresaDto
    {
        public string Nombre { get; set; }
        public decimal Coste { get; set; }
    }
    public class FacturaMaterialDto
    {
        public string Nombre { get; set; }
        public decimal Coste { get; set; }
    }
    public class FacturaDetalleDto
    {
        public FacturaEmpresaDto Empresa { get; set; }
        public List<FacturaMaterialDto> Materiales { get; set; }
        public decimal TotalConIVA { get; set; }
    }
    public class FacturaResponse
    {
        public bool Success { get; set; }
        public FacturaDetalleDto Factura { get; set; }
        public string Error { get; set; }
    }
}
