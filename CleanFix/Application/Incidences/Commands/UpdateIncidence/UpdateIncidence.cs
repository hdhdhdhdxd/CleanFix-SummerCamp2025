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
        var entity = _mapper.Map<Incidence>(request.Incidence);
        
        _incidenceRepository.Update(entity);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
