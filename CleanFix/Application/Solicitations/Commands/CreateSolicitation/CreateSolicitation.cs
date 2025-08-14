using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

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
        var solicitation = _mapper.Map<Solicitation>(request.Solicitation);

        if (solicitation.Id == 0)
            solicitation.Id = 0;

        var result = _solicitationRepository.Add(solicitation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result;
    }
}
