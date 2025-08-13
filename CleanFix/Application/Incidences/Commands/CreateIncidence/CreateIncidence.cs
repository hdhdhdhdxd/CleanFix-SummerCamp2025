using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Incidences.Commands.CreateIncidence;

public record CreateIncidenceCommand : IRequest<int>
{
    public CreateIncidenceDto Incidence { get; init; }
}
public class CreateIncidenceCommandHandler : IRequestHandler<CreateIncidenceCommand, int>
{
    private readonly IIncidenceRepository _incidenceRepository;
    private readonly IMapper _mapper;

    public CreateIncidenceCommandHandler(IIncidenceRepository incidenceRepository, IMapper mapper)
    {
        _incidenceRepository = incidenceRepository;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateIncidenceCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Incidence>(request.Incidence);
        if (entity.Id == 0)
            entity.Id = 0;
        var result = await _incidenceRepository.AddAsync(entity, cancellationToken);
        return result;
    }
}
