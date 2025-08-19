using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Application.CompletedTasks.Queries.GetCompletedTasks;
public record GetCompletedTasksQuery : IRequest<List<GetCompletedTasksDto>>;

public class GetCompletedTasksQueryHandler : IRequestHandler<GetCompletedTasksQuery, List<GetCompletedTasksDto>>
{
    private readonly ICompletedTaskRepository _completedTaskRepository;
    private readonly IMapper _mapper;

    public GetCompletedTasksQueryHandler(ICompletedTaskRepository completedTaskRepository, IMapper mapper)
    {
        _completedTaskRepository = completedTaskRepository;
        _mapper = mapper;
    }

    public async Task<List<GetCompletedTasksDto>> Handle(GetCompletedTasksQuery request, CancellationToken cancellationToken)
    {
        var completedTasks = await _completedTaskRepository.GetAll()
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        var result = _mapper.Map<List<GetCompletedTasksDto>>(completedTasks);
        return result;
    }
}
