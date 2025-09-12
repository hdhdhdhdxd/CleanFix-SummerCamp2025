using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Common.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CompletedTasks.Queries.GetPaginatedCompletedTasks;

[Authorize(Roles = Roles.Administrator)]
public record GetPaginatedCompletedTasksQuery(int PageNumber, int PageSize, string? FilterString) : IRequest<PaginatedList<GetPaginatedCompletedTaskDto>>;

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
        var query = _completedTaskRepository.GetQueryable()
            .Include(ct => ct.Company)
            .Include(ct => ct.IssueType)
            .AsNoTracking();

        if (!string.IsNullOrWhiteSpace(request.FilterString))
        {
            var filter = request.FilterString.ToLower();
            query = query.Where(ct =>
                (ct.Address != null && ct.Address.ToLower().Contains(filter)) ||
                (ct.Company != null && ct.Company.Name.ToLower().Contains(filter)) ||
                (ct.IssueType != null && ct.IssueType.Name.ToLower().Contains(filter)) ||
                ct.CreationDate.ToString().ToLower().Contains(filter) ||
                ct.CompletionDate.ToString().ToLower().Contains(filter) ||
                (ct.IsSolicitation && "solicitud".Contains(filter)) ||
                (!ct.IsSolicitation && "incidencia".Contains(filter))
            );
        }

        var completedTasks = await query
            .OrderByDescending(i => i.CreationDate)
            .ProjectTo<GetPaginatedCompletedTaskDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return completedTasks;
    }
}
