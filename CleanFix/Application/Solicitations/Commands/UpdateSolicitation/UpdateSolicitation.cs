using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Solicitations.Commands.UpdateSolicitation;

public record UpdateSolicitationCommand : IRequest
{
    public UpdateSolicitationDto Solicitation { get; init; }
}

public class UpdateSolicitationCommandHandler : IRequestHandler<UpdateSolicitationCommand>
{
    private readonly ISolicitationRepository _solicitationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateSolicitationCommandHandler(ISolicitationRepository solicitationRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _solicitationRepository = solicitationRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateSolicitationCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Solicitation>(request.Solicitation);
        try
        {
            _solicitationRepository.Update(entity);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            // Manejo de conflicto de concurrencia
            throw new Exception("Conflicto de concurrencia: el registro fue modificado por otro usuario. Por favor, recargue y vuelva a intentar.");
        }
    }
}
