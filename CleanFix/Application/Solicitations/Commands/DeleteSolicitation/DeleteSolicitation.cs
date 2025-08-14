using Application.Common.Interfaces;
using MediatR;

namespace Application.Solicitations.Commands.DeleteSolicitation;

public record DeleteSolicitationCommand(int Id) : IRequest<bool>;

public class DeleteSolicitationCommandHandler : IRequestHandler<DeleteSolicitationCommand, bool>
{
    private readonly ISolicitationRepository _solicitationRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSolicitationCommandHandler(ISolicitationRepository solicitationRepository, IUnitOfWork unitOfWork)
    {
        _solicitationRepository = solicitationRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteSolicitationCommand request, CancellationToken cancellationToken)
    {
        var solicitation = await _solicitationRepository.GetByIdAsync(request.Id);

        if (solicitation == null)
            return false;

        _solicitationRepository.Remove(solicitation);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
