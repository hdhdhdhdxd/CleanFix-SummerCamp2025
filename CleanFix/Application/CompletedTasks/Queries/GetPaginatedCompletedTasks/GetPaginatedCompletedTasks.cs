using Application.Common.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Common.Mappings;
using Infrastructure.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompletedTasks.Queries.GetPaginatedCompletedTasks;
public record GetPaginatedCompletedTasksQuery(int PageNumber, int PageSize) : IRequest<PaginatedList<GetPaginatedCompletedTaskDto>>;

public class GetPaginatedCompletedTasksQueryHandler : IRequestHandler<GetPaginatedCompletedTasksQuery, PaginatedList<GetPaginatedCompletedTaskDto>>
{
    private readonly ICompletedTaskRepository _completedTaskRepository;
    private readonly IMapper _mapper;

    public GetPaginatedCompletedTasksQueryHandler(ICompletedTaskRepository completedTaskRepository, IMapper mapper)
    {
        _completedTaskRepository = completedTaskRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedList<GetPaginatedCompletedTaskDto>> Handle(GetPaginatedCompletedTasksQuery request, CancellationToken cancellationToken)
    {
        var completedTasks = await _completedTaskRepository.GetAll()
            .AsNoTracking()
            .ProjectTo<GetPaginatedCompletedTaskDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return completedTasks;
    }
}
