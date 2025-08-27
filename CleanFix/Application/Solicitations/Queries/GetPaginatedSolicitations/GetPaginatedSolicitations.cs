using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Solicitations.Queries.GetPaginatedSolicitations;
public record GetPaginatedSolicitationsQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<GetPaginatedSolicitationDto>>;

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
        var solicitations = await _solicitationRepository.GetQueryable()
            .AsNoTracking()
            .ProjectTo<GetPaginatedSolicitationDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return solicitations;
    }
}
