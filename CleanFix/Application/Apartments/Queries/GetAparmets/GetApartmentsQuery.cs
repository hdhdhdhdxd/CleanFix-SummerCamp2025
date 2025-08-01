using Application.Common.Interfaces;

namespace Application.Apartments.Queries.GetAparmets;
public class GetApartmentsQuery : IGetApartmentsQuery
{
    private readonly IApartmentRepository _apartmentRepository;

    public GetApartmentsQuery(IApartmentRepository apartmentRepository)
    {
        _apartmentRepository = apartmentRepository;
    }

    public async Task<IEnumerable<GetApartmentsDto>> ExecuteAsync()
    {
        var apartments = _apartmentRepository.GetAll();

        return await Task.FromResult(apartments.Select(a => new GetApartmentsDto
        {
            Id = a.Id,
            Address = a.Address,
        }));
    }
}
