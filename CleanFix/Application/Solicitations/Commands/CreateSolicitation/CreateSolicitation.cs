using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Application.Common.Utils;

namespace Application.Solicitations.Commands.CreateSolicitation;

public record CreateSolicitationCommand : IRequest<int>
{
    public CreateSolicitationDto Solicitation { get; init; }
}
public class CreateSolicitationCommandHandler : IRequestHandler<CreateSolicitationCommand, int>
{
    private readonly ISolicitationRepository _solicitationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateSolicitationCommandHandler(ISolicitationRepository solicitationRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _solicitationRepository = solicitationRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateSolicitationCommand request, CancellationToken cancellationToken)
    {
        // Normalización antes de mapear y guardar
        if (!string.IsNullOrEmpty(request.Solicitation.Address))
            request.Solicitation.Address = Normalizer.NormalizarNombre(request.Solicitation.Address);

        var entity = _mapper.Map<Solicitation>(request.Solicitation);
        entity.Date = DateTime.UtcNow; // Asignar fecha de creación

        // Asignar un IssueTypeId aleatorio entre 1 y 7
        entity.IssueTypeId = Random.Shared.Next(1, 8);
        
        if (entity.Id == 0)
            entity.Id = 0;
        _solicitationRepository.Add(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return entity.Id;
    }
}
