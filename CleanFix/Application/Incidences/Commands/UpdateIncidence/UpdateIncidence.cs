using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Utils;

namespace Application.Incidences.Commands.UpdateIncidence;

public record UpdateIncidenceCommand : IRequest
{
    public UpdateIncidenceDto Incidence { get; init; }
}

public class UpdateIncidenceCommandHandler : IRequestHandler<UpdateIncidenceCommand>
{
    private readonly IIncidenceRepository _incidenceRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateIncidenceCommandHandler(IIncidenceRepository incidenceRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _incidenceRepository = incidenceRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateIncidenceCommand request, CancellationToken cancellationToken)
    {
        // Normalización antes de mapear y actualizar
        if (request.Incidence.Status != null)
            request.Incidence.Status = Normalizer.NormalizarNombre(request.Incidence.Status);
        if (request.Incidence.Description != null)
            request.Incidence.Description = Normalizer.NormalizarNombre(request.Incidence.Description);

        var entity = _mapper.Map<Incidence>(request.Incidence);
        try
        {
            _incidenceRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new Exception("Conflicto de concurrencia: el registro fue modificado por otro usuario. Por favor, recargue y vuelva a intentar.");
        }
    }
}
