using Application.Common.Models;

namespace Application.Common.Interfaces;
public interface IExternalIncidenceRepository
{
    Task<Result> UpdateIncidenceCost(int incidenceId, string companyName);
}
