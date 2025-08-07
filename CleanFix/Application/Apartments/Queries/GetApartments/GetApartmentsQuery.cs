using MediatR;
using WebApi.Interfaces;
using AutoMapper;

namespace Application.Apartments.Queries.GetApartments;

public record GetApartmentsQuery : IRequest<List<GetApartmentDto>>;

public class GetApartmentsQueryHandler : IRequestHandler<GetApartmentsQuery, List<GetApartmentDto>>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IMapper _mapper;

    public GetApartmentsQueryHandler(IApartmentRepository apartmentRepository, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
    }

    public async Task<List<GetApartmentDto>> Handle(GetApartmentsQuery request, CancellationToken cancellationToken)
    {
        var apartments = _apartmentRepository.GetAll().ToList();

        var result = _mapper.Map<List<GetApartmentDto>>(apartments);

        return await Task.FromResult(result);
    }
}
