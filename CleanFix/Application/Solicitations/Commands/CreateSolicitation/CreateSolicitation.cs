using AutoMapper;
using Domain.Entities;
using MediatR;
using WebApi.Interfaces;

namespace Application.Solicitations.Commands.CreateSolicitation;

public record CreateSolicitationCommand : IRequest<Guid>
{
    public CreateSolicitationDto Solicitation { get; init; }
}
public class CreateSolicitationCommandHandler : IRequestHandler<CreateSolicitationCommand, Guid>
{
    private readonly ISolicitationRepository _solicitationRepository;
    private readonly IMapper _mapper;

    public CreateSolicitationCommandHandler(ISolicitationRepository solicitationRepository, IMapper mapper)
    {
        _solicitationRepository = solicitationRepository;
        _mapper = mapper;
    }

    public async Task<Guid> Handle(CreateSolicitationCommand request, CancellationToken cancellationToken)
    {
        var solicitation = _mapper.Map<Solicitation>(request.Solicitation);

        if (solicitation.Id == Guid.Empty)
            solicitation.Id = Guid.NewGuid();

        var result = await _solicitationRepository.AddAsync(solicitation, cancellationToken);

        return result;
    }
}
