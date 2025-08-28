using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
        var query = _completedTaskRepository.GetQueryable()
            .Include(ct => ct.Materials)
            .Include(ct => ct.Company)
            .Include(ct => ct.IssueType)
            .Include(ct => ct.Apartment)
            .AsNoTracking();

        var completedTask = await query
            .FirstOrDefaultAsync(ct => ct.Id == request.Id, cancellationToken);

        var result = _mapper.Map<GetCompletedTaskDto>(completedTask);
        return result;
    }
}
