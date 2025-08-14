using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.Solicitations.Queries.GetSolicitations;
public record GetSolicitationsQuery : IRequest<List<GetSolicitationsDto>>;

public class GetSolicitationsQueryHandler : IRequestHandler<GetSolicitationsQuery, List<GetSolicitationsDto>>
{
    private readonly ISolicitationRepository _solicitationRepository;
    private readonly IMapper _mapper;

    public GetSolicitationsQueryHandler(ISolicitationRepository solicitationRepository, IMapper mapper)
    {
        _solicitationRepository = solicitationRepository;
        _mapper = mapper;
    }

    public async Task<List<GetSolicitationsDto>> Handle(GetSolicitationsQuery request, CancellationToken cancellationToken)
    {
        var solicitations = await _solicitationRepository.GetAll()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var result = _mapper.Map<List<GetSolicitationsDto>>(solicitations);
        return result;
    }
}
