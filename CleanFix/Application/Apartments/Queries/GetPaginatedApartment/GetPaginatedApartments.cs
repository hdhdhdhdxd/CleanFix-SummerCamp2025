using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Apartments.Queries.GetPaginatedApartment;
public record GetPaginatedApartmentsQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<GetPaginatedApartmentDto>>;

public class GetPaginatedApartmentsQueryHandler : IRequestHandler<GetPaginatedApartmentsQuery, PaginatedList<GetPaginatedApartmentDto>>
{
    private readonly IApartmentRepository _apartmentRepository;
    private readonly IMapper _mapper;

    public GetPaginatedApartmentsQueryHandler(IApartmentRepository apartmentRepository, IMapper mapper)
    {
        _apartmentRepository = apartmentRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedApartmentDto>> Handle(GetPaginatedApartmentsQuery request, CancellationToken cancellationToken)
    {
        var apartments = await _apartmentRepository.GetQueryable()
            .AsNoTracking()
            .ProjectTo<GetPaginatedApartmentDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return apartments;
    }
}
