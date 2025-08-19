using Application.Common.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.CompletedTasks.Queries.GetCompletedTask;
public record GetCompletedTaskQuery(int Id) : IRequest<GetCompletedTaskDto>;

public class GetCompletedTaskQueryHandler : IRequestHandler<GetCompletedTaskQuery, GetCompletedTaskDto>
{
    private readonly ICompletedTaskRepository _completedTaskRepository;
    private readonly IMapper _mapper;

    public GetCompletedTaskQueryHandler(ICompletedTaskRepository completedTaskRepository, IMapper mapper)
    {
        _completedTaskRepository = completedTaskRepository;
        _mapper = mapper;
    }

    public async Task<GetCompletedTaskDto> Handle(GetCompletedTaskQuery request, CancellationToken cancellationToken)
    {
        var completedTask = await _completedTaskRepository.GetByIdAsync(request.Id);
        var result = _mapper.Map<GetCompletedTaskDto>(completedTask);
        return result;
    }
}
