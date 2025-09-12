using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Incidences.Queries.GetIncidence;

[Authorize(Roles = Roles.Administrator)]
public record GetIncidenceQuery(int Id) : IRequest<GetIncidenceDto>;

public class GetIncidenceQueryHandler : IRequestHandler<GetIncidenceQuery, GetIncidenceDto>
{
    private readonly IIncidenceRepository _incidenceRepository;
    private readonly IMapper _mapper;

    public GetIncidenceQueryHandler(IIncidenceRepository incidenceRepository, IMapper mapper)
    {
        _incidenceRepository = incidenceRepository;
        _mapper = mapper;
    }

    public async Task<GetIncidenceDto> Handle(GetIncidenceQuery request, CancellationToken cancellationToken)
    {
        var entity = await _incidenceRepository.GetQueryable()
            .AsNoTracking()
            .Include(s => s.IssueType)
            .FirstOrDefaultAsync(p => p.Id == request.Id);

        var result = _mapper.Map<GetIncidenceDto>(entity);

        return result;
    }
}
