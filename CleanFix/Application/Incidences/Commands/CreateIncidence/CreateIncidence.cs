using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Application.Common.Utils;

namespace Application.Incidences.Commands.CreateIncidence;

public record CreateIncidenceCommand : IRequest<int>
{
    public CreateIncidenceDto Incidence { get; init; }
}
public class CreateIncidenceCommandHandler : IRequestHandler<CreateIncidenceCommand, int>
{
    private readonly IIncidenceRepository _incidenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateIncidenceCommandHandler(IIncidenceRepository incidenceRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _incidenceRepository = incidenceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateIncidenceCommand request, CancellationToken cancellationToken)
    {
        // Normalización antes de mapear y guardar
        if (request.Incidence.Description != null)
            request.Incidence.Description = Normalizer.NormalizarNombre(request.Incidence.Description);

        var entity = _mapper.Map<Incidence>(request.Incidence);
        entity.Date = DateTime.UtcNow; // Asignar fecha de creación
        if (entity.Id == 0)
            entity.Id = 0;
        _incidenceRepository.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
