using AutoMapper;
using MediatR;
using WebApi.Interfaces;

namespace Application.Apartments.Queries.GetApartment;
public record GetApartmentQuery : IRequest<GetApartmentDto>
{
    public Guid Id { get; init; }
}

public class GetApartmentQueryHandler : IRequestHandler<GetApartmentQuery, GetApartmentDto>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IMapper _mapper;

    public GetApartmentQueryHandler(IApartmentRepository apartmentRepository, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
    }

    public async Task<GetApartmentDto> Handle(GetApartmentQuery request, CancellationToken cancellationToken)
    {
        var apartment = await _apartmentRepository.GetByIdAsync(request.Id, cancellationToken);

        var result = _mapper.Map<GetApartmentDto>(apartment);

        return result;
    }
}
