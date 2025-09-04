using Application.Common.Models;

namespace Application.Common.Interfaces;
public interface IRequestRepository
{
    Task<Result> UpdateRequestCost(string buildingCode, decimal cost);
}
