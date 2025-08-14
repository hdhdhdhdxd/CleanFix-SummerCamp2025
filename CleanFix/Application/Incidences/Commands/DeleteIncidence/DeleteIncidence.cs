using Application.Common.Interfaces;
using MediatR;

namespace Application.Incidences.Commands.DeleteIncidence;

public record DeleteIncidenceCommand(int Id) : IRequest<bool>;

public class DeleteIncidenceCommandHandler : IRequestHandler<DeleteIncidenceCommand, bool>
{
    private readonly IIncidenceRepository _incidenceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteIncidenceCommandHandler(IIncidenceRepository incidenceRepository, IUnitOfWork unitOfWork)
    {
        _incidenceRepository = incidenceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteIncidenceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _incidenceRepository.GetByIdAsync(request.Id);

        if (entity == null)
            return false;

        _incidenceRepository.Remove(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
