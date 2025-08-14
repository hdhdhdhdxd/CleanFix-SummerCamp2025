using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Common.Mappings;
using Infrastructure.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Incidences.Queries.GetPaginatedIncidences;
public record GetPaginatedIncidencesQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<GetPaginatedIncidenceDto>>;

public class GetPaginatedIncidencesQueryHandler : IRequestHandler<GetPaginatedIncidencesQuery, PaginatedList<GetPaginatedIncidenceDto>>
{
    private readonly IIncidenceRepository _incidenceRepository;
    private readonly IMapper _mapper;

    public GetPaginatedIncidencesQueryHandler(IIncidenceRepository incidenceRepository, IMapper mapper)
    {
        _incidenceRepository = incidenceRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedIncidenceDto>> Handle(GetPaginatedIncidencesQuery request, CancellationToken cancellationToken)
    {
        var incidences = await _incidenceRepository.GetAll()
            .AsNoTracking()
            .ProjectTo<GetPaginatedIncidenceDto>(_mapper.ConfigurationProvider).PaginatedListAsync(request.PageNumber, request.PageSize);

        return incidences;
    }
}
