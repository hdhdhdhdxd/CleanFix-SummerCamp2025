using Application.Common.Interfaces;
using MediatR;

namespace Application.Requests.Commands.DeleteRequest;

public record DeleteRequestCommand(int Id) : IRequest<bool>;

public class DeleteRequestCommandHandler : IRequestHandler<DeleteRequestCommand, bool>
{
    private readonly IRequestRepository _requestRepository;

    public DeleteRequestCommandHandler(IRequestRepository requestRepository)
    {
        _requestRepository = requestRepository;
    }

    public async Task<bool> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await _requestRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null)
            return false;
        await _requestRepository.RemoveAsync(entity, cancellationToken);
        return true;
    }
}
