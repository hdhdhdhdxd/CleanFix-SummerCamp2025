using Application.Common.Interfaces;
using MediatR;

namespace Application.CompletedTasks.Commands.DeleteCompletedTask;

public record DeleteCompletedTaskCommand(int Id) : IRequest<bool>;

public class DeleteCompletedTaskCommandHandler : IRequestHandler<DeleteCompletedTaskCommand, bool>
{
    private readonly ICompletedTaskRepository _completedTaskRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCompletedTaskCommandHandler(ICompletedTaskRepository completedTaskRepository, IUnitOfWork unitOfWork)
    {
        _completedTaskRepository = completedTaskRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(DeleteCompletedTaskCommand request, CancellationToken cancellationToken)
    {
        var completedTask = await _completedTaskRepository.GetByIdAsync(request.Id);

        if (completedTask == null)
            return false;

        _completedTaskRepository.Remove(completedTask);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }
}
