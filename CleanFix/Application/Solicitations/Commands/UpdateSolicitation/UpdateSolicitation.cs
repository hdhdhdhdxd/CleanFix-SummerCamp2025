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
    private readonly IMapper _mapper;

    public UpdateSolicitationCommandHandler(ISolicitationRepository solicitationRepository, IMapper mapper)
    {
        _solicitationRepository = solicitationRepository;
        _mapper = mapper;
    }

    public async Task Handle(UpdateSolicitationCommand request, CancellationToken cancellationToken)
    {
        var solicitation = _mapper.Map<Solicitation>(request.Solicitation);

        await _solicitationRepository.UpdateAsync(solicitation, cancellationToken);
    }
}
