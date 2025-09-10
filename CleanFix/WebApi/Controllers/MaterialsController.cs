using Application.Materials.Queries.GetRandomMaterialsByIssueType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/materials")]
    [ApiController]
    [Authorize]
    public class MaterialsController : ControllerBase
    {
        private readonly ISender _sender;

        public MaterialsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("random")]
        public async Task<ActionResult<List<GetRandomMaterialDto>>> GetRandomMaterials([FromQuery] int issueTypeId)
        {
            Log.Information("GET api/materials/random called. IssueTypeId={IssueTypeId}", issueTypeId);
            var result = await _sender.Send(new GetRandomMaterialsByIssueTypeQuery(issueTypeId));
            Log.Information("GET api/materials/random returned {Count} results.", result.Count);
            return Ok(result);
        }
    }
}
