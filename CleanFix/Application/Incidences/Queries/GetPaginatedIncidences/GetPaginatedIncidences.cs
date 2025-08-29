using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        var incidences = await _incidenceRepository.GetQueryable()
            .Include(i => i.IssueType)
            .AsNoTracking()
            .OrderByDescending(i => i.Date)
            .ProjectTo<GetPaginatedIncidenceDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return incidences;
    }
}
