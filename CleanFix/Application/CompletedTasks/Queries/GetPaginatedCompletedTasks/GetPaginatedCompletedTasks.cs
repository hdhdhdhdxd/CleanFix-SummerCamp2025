using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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
        var completedTasks = await _completedTaskRepository.GetQueryable()
            .Include(ct => ct.Company)
            .Include(ct => ct.IssueType)
            .AsNoTracking()
            .OrderByDescending(i => i.CreationDate)
            .ProjectTo<GetPaginatedCompletedTaskDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return completedTasks;
    }
}
