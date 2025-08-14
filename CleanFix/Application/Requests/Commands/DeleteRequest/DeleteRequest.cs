using Application.Common.Interfaces;
using MediatR;

namespace Application.Requests.Commands.DeleteRequest;

public record DeleteRequestCommand(int Id) : IRequest<bool>;

public class DeleteRequestCommandHandler : IRequestHandler<DeleteRequestCommand, bool>
{
    private readonly IRequestRepository _requestRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRequestCommandHandler(IRequestRepository requestRepository, IUnitOfWork unitOfWork)
    {
        _requestRepository = requestRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteRequestCommand request, CancellationToken cancellationToken)
    {
        var entity = await _requestRepository.GetByIdAsync(request.Id);

        if (entity == null)
            return false;

        _requestRepository.Remove(entity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
