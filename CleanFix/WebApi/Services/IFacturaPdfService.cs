using System.IO;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IFacturaPdfService
    {
        Task<byte[]> GenerarFacturaPdfAsync(FacturaDetalleDto factura);
    }
}
