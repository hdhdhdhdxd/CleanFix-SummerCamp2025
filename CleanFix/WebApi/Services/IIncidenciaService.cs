using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Services
{
    public interface IIncidenciaService
    {
        Task ReportarIncidenciaAsync(string usuario, string descripcion, string tipo, string referencia);
    }
}
