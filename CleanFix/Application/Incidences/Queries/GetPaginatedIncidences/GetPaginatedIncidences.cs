using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Incidences.Queries.GetPaginatedIncidences;
public record GetPaginatedIncidencesQuery(int PageNumber, int PageSize, string? FilterString) : IRequest<PaginatedList<GetPaginatedIncidenceDto>>;

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
        var query = _incidenceRepository.GetQueryable()
            .Include(i => i.IssueType)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.FilterString))
        {
            var filter = request.FilterString.Trim().ToLower();
            query = query.Where(i =>
                i.Address.ToLower().Contains(filter) ||
                i.IssueType.Name.ToLower().Contains(filter) ||
                i.Date.ToString().ToLower().Contains(filter) ||
                ("baja".Contains(filter) && i.Priority == Priority.Low) ||
                ("media".Contains(filter) && i.Priority == Priority.Medium) ||
                ("alta".Contains(filter) && i.Priority == Priority.High) ||
                ("urgente".Contains(filter) && i.Priority == Priority.Critical)
            );
        }

        var incidences = await query
            .OrderByDescending(i => i.Date)
            .ProjectTo<GetPaginatedIncidenceDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return incidences;
    }
}
