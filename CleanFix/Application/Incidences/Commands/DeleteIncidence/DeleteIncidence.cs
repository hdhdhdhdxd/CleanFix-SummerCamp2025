using Application.Common.Interfaces;
using MediatR;

namespace Application.Incidences.Commands.DeleteIncidence;

public record DeleteIncidenceCommand(Guid Id) : IRequest<bool>;

public class DeleteIncidenceCommandHandler : IRequestHandler<DeleteIncidenceCommand, bool>
{
    private readonly IIncidenceRepository _incidenceRepository;

    public DeleteIncidenceCommandHandler(IIncidenceRepository incidenceRepository)
    {
        _incidenceRepository = incidenceRepository;
    }

    public async Task<bool> Handle(DeleteIncidenceCommand request, CancellationToken cancellationToken)
    {
        var entity = await _incidenceRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return false;
        await _incidenceRepository.RemoveAsync(entity, cancellationToken);
        return true;
    }
}
