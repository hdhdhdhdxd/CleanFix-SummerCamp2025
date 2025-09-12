using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Solicitations.Queries.GetPaginatedSolicitations;

[Authorize(Roles = Roles.Administrator)]
public record GetPaginatedSolicitationsQuery(int PageNumber, int PageSize, string? FilterString) : IRequest<PaginatedList<GetPaginatedSolicitationDto>>;

public class GetPaginatedSolicitationsQueryHandler : IRequestHandler<GetPaginatedSolicitationsQuery, PaginatedList<GetPaginatedSolicitationDto>>
{
    private readonly ISolicitationRepository _solicitationRepository;
    private readonly IMapper _mapper;

    public GetPaginatedSolicitationsQueryHandler(ISolicitationRepository solicitationRepository, IMapper mapper)
    {
        _solicitationRepository = solicitationRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedSolicitationDto>> Handle(GetPaginatedSolicitationsQuery request, CancellationToken cancellationToken)
    {
        var query = _solicitationRepository.GetQueryable()
            .Include(s => s.IssueType)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.FilterString))
        {
            var filter = request.FilterString.ToLower();
            query = query.Where(s =>
                (s.Address != null && s.Address.ToLower().Contains(filter)) ||
                s.Date.ToString().ToLower().Contains(filter) ||
                s.IssueType.Name.ToLower().Contains(filter)
            );
        }

        var solicitations = await query
            .OrderByDescending(i => i.Date)
            .ProjectTo<GetPaginatedSolicitationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return solicitations;
    }
}
