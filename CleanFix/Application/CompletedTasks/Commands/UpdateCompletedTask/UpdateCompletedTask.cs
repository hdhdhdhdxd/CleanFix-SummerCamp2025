using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.CompletedTasks.Commands.UpdateCompletedTask;

public record UpdateCompletedTaskCommand : IRequest
{
    public UpdateCompletedTaskDto CompletedTask { get; init; }
}

public class UpdateCompletedTaskCommandHandler : IRequestHandler<UpdateCompletedTaskCommand>
{
    private readonly ICompletedTaskRepository _completedTaskRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateCompletedTaskCommandHandler(ICompletedTaskRepository completedTaskRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _completedTaskRepository = completedTaskRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateCompletedTaskCommand request, CancellationToken cancellationToken)
    {
        var completedTask = _mapper.Map<CompletedTask>(request.CompletedTask);

        _completedTaskRepository.Update(completedTask);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
