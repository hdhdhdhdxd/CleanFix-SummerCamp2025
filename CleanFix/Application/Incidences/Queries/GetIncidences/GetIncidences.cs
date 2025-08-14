using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.Incidences.Queries.GetIncidences;
public record GetIncidencesQuery : IRequest<List<GetIncidencesDto>>;

public class GetIncidencesQueryHandler : IRequestHandler<GetIncidencesQuery, List<GetIncidencesDto>>
{
    private readonly IIncidenceRepository _incidenceRepository;
    private readonly IMapper _mapper;

    public GetIncidencesQueryHandler(IIncidenceRepository incidenceRepository, IMapper mapper)
    {
        _incidenceRepository = incidenceRepository;
        _mapper = mapper;
    }

    public async Task<List<GetIncidencesDto>> Handle(GetIncidencesQuery request, CancellationToken cancellationToken)
    {
        var entities = await _incidenceRepository.GetAll()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var result = _mapper.Map<List<GetIncidencesDto>>(entities);
        return result;
    }
}
