using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Common.Mappings;

namespace Application.Apartments.Queries.GetApartments;

public record GetApartmentsQuery : IRequest<List<GetApartmentsDto>>;

public class GetApartmentsQueryHandler : IRequestHandler<GetApartmentsQuery, List<GetApartmentsDto>>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IMapper _mapper;

    public GetApartmentsQueryHandler(IApartmentRepository apartmentRepository, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
    }

    public async Task<List<GetApartmentsDto>> Handle(GetApartmentsQuery request, CancellationToken cancellationToken)
    {
        var apartments = await _apartmentRepository.GetAll().ToListAsync();

        var result = _mapper.Map<List<GetApartmentsDto>>(apartments);

        return result;
    }
}
