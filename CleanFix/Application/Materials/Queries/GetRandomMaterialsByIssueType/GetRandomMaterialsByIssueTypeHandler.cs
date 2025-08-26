using MediatR;
using Application.Common.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Materials.Queries.GetRandomMaterialsByIssueType
{
    public class GetRandomMaterialsByIssueTypeHandler : IRequestHandler<GetRandomMaterialsByIssueTypeQuery, List<GetRandomMaterialDto>>
    {
        private readonly IMaterialRepository _materialRepository;

        public GetRandomMaterialsByIssueTypeHandler(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<List<GetRandomMaterialDto>> Handle(GetRandomMaterialsByIssueTypeQuery request, CancellationToken cancellationToken)
        {
            var materials = _materialRepository.GetQueryable();
            var filtered = materials.Where(m => m.IssueTypeId == request.IssueTypeId && m.Available).ToList();
            var random = filtered.OrderBy(x => Guid.NewGuid()).Take(request.Count).ToList();
            return random.Select(m => new GetRandomMaterialDto
            {
                Id = m.Id,
                Name = m.Name,
                Cost = m.Cost,
                Available = m.Available,
                IssueTypeId = m.IssueTypeId
            }).ToList();
        }
    }
}
