using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CompletedTasks.Commands.CreateCompletedTask;

public record CreateCompletedTaskCommand : IRequest<int>
{
    public CreateCompletedTaskDto CompletedTask { get; init; }
}
public class CreateCompletedTaskCommandHandler : IRequestHandler<CreateCompletedTaskCommand, int>
{
    private readonly ICompletedTaskRepository _completedTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCompletedTaskCommandHandler(ICompletedTaskRepository completedTaskRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _completedTaskRepository = completedTaskRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateCompletedTaskCommand request, CancellationToken cancellationToken)
    {
        var completedTask = _mapper.Map<CompletedTask>(request.CompletedTask);
        completedTask.CreationDate = DateTime.UtcNow; // Asignar fecha de creación

        if (completedTask.Id == 0)
            completedTask.Id = 0;

        _completedTaskRepository.Add(completedTask);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return completedTask.Id;
    }
}
