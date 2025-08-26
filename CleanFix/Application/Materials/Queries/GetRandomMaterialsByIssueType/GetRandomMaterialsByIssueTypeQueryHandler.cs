using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Materials.Queries.GetRandomMaterialsByIssueType;

public class GetRandomMaterialsByIssueTypeQuery : IRequest<List<GetRandomMaterialDto>>
{
    public int IssueTypeId { get; set; }
    public int Count { get; set; } = 3;
    public GetRandomMaterialsByIssueTypeQuery(int issueTypeId, int count = 3)
    {
        IssueTypeId = issueTypeId;
        Count = count;
    }
}

public class GetRandomMaterialsByIssueTypeQueryHandler : IRequestHandler<GetRandomMaterialsByIssueTypeQuery, List<GetRandomMaterialDto>>
{
    private readonly IMaterialRepository _materialRepository;

    public GetRandomMaterialsByIssueTypeQueryHandler(IMaterialRepository materialRepository)
    {
        _materialRepository = materialRepository;
    }

    public async Task<List<GetRandomMaterialDto>> Handle(GetRandomMaterialsByIssueTypeQuery request, CancellationToken cancellationToken)
    {
        var query = _materialRepository.GetQueryable();

        var result = await query.Where(m => m.IssueTypeId == request.IssueTypeId && m.Available)
                          .OrderBy(x => Guid.NewGuid()).Take(request.Count)
                          .Select(m => new GetRandomMaterialDto
                          {
                              Id = m.Id,
                              Name = m.Name,
                              Cost = m.Cost,
                              Available = m.Available,
                              IssueTypeId = m.IssueTypeId
                          }).ToListAsync();

        return result;
    }
}

