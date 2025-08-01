namespace Application.Apartments.Queries.GetAparmets;
public interface IGetApartmentsQuery
{
    Task<IEnumerable<GetApartmentsDto>> ExecuteAsync();
}
