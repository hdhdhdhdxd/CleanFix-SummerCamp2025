using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Solicitations.Queries.GetSolicitation;

[Authorize(Roles = Roles.Administrator)]
public record GetSolicitationQuery(int Id) : IRequest<GetSolicitationDto>;

public class GetSolicitationQueryHandler : IRequestHandler<GetSolicitationQuery, GetSolicitationDto>
{
    private readonly ISolicitationRepository _solicitationRepository;
    private readonly IMapper _mapper;

    public GetSolicitationQueryHandler(ISolicitationRepository solicitationRepository, IMapper mapper)
    {
        _solicitationRepository = solicitationRepository;
        _mapper = mapper;
    }

    public async Task<GetSolicitationDto> Handle(GetSolicitationQuery request, CancellationToken cancellationToken)
    {
        var entity = await _solicitationRepository.GetQueryable()
            .AsNoTracking()
            .Include(s => s.IssueType)
            .FirstOrDefaultAsync(p => p.Id == request.Id);

        var result = _mapper.Map<GetSolicitationDto>(entity);

        return result;
    }
}
