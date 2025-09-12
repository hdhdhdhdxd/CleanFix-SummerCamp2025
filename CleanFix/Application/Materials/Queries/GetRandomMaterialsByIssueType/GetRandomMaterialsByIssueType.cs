using Application.Common.Interfaces;
using Application.Common.Security;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain.Constants;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Materials.Queries.GetRandomMaterialsByIssueType;

[Authorize(Roles = Roles.Administrator)]
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

public class GetRandomMaterialsByIssueType : IRequestHandler<GetRandomMaterialsByIssueTypeQuery, List<GetRandomMaterialDto>>
{
    private readonly IMaterialRepository _materialRepository;
    private readonly IMapper _mapper;

    public GetRandomMaterialsByIssueType(IMaterialRepository materialRepository, IMapper mapper)
    {
        _materialRepository = materialRepository;
        _mapper = mapper;
    }

    public async Task<List<GetRandomMaterialDto>> Handle(GetRandomMaterialsByIssueTypeQuery request, CancellationToken cancellationToken)
    {
        var query = _materialRepository.GetQueryable();

        var result = await query.Where(m => m.IssueTypeId == request.IssueTypeId && m.Available)
                          .OrderBy(x => Guid.NewGuid())
                          .Take(request.Count)
                          .ProjectTo<GetRandomMaterialDto>(_mapper.ConfigurationProvider)
                          .ToListAsync();

        return result;
    }
}

