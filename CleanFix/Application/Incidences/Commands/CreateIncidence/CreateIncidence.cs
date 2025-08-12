using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Incidences.Commands.CreateIncidence;

public record CreateIncidenceCommand : IRequest<Guid>
{
    public CreateIncidenceDto Incidence { get; init; }
}
public class CreateIncidenceCommandHandler : IRequestHandler<CreateIncidenceCommand, Guid>
{
    private readonly IIncidenceRepository _incidenceRepository;
    private readonly IMapper _mapper;

    public CreateIncidenceCommandHandler(IIncidenceRepository incidenceRepository, IMapper mapper)
    {
        _incidenceRepository = incidenceRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateIncidenceCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Incidence>(request.Incidence);
        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();
        var result = await _incidenceRepository.AddAsync(entity, cancellationToken);
        return result;
    }
}
