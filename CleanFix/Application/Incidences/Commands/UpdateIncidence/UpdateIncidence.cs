using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Incidences.Commands.UpdateIncidence;

public record UpdateIncidenceCommand : IRequest
{
    public UpdateIncidenceDto Incidence { get; init; }
}

public class UpdateIncidenceCommandHandler : IRequestHandler<UpdateIncidenceCommand>
{
    private readonly IIncidenceRepository _incidenceRepository;
    private readonly IMapper _mapper;

    public UpdateIncidenceCommandHandler(IIncidenceRepository incidenceRepository, IMapper mapper)
    {
        _incidenceRepository = incidenceRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateIncidenceCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Incidence>(request.Incidence);
        await _incidenceRepository.UpdateAsync(entity, cancellationToken);
    }
}
