using Application.Common.Interfaces;
using MediatR;

namespace Application.Solicitations.Commands.DeleteSolicitation;

public record DeleteSolicitationCommand(Guid Id) : IRequest<bool>;

public class DeleteSolicitationCommandHandler : IRequestHandler<DeleteSolicitationCommand, bool>
{
    private readonly ISolicitationRepository _solicitationRepository;

    public DeleteSolicitationCommandHandler(ISolicitationRepository solicitationRepository)
    {
        _solicitationRepository = solicitationRepository;
    }

    public async Task<bool> Handle(DeleteSolicitationCommand request, CancellationToken cancellationToken)
    {
        var solicitation = await _solicitationRepository.GetByIdAsync(request.Id, cancellationToken);

        if (solicitation == null)
            return false;

        await _solicitationRepository.RemoveAsync(solicitation, cancellationToken);

        return true;
    }
}
