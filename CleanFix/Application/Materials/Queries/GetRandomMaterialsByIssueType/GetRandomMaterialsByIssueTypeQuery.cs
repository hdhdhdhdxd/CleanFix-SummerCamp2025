using MediatR;
using System.Collections.Generic;

namespace Application.Materials.Queries.GetRandomMaterialsByIssueType
{
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
}
