using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

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
        var solicitation = _mapper.Map<Solicitation>(request.Solicitation);

        _solicitationRepository.Update(solicitation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
