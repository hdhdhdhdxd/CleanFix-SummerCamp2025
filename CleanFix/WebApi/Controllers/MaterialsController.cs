using Application.Materials.Queries.GetRandomMaterialsByIssueType;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/materials")]
    [ApiController]
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
            var result = await _sender.Send(new GetRandomMaterialsByIssueTypeQuery(issueTypeId));
            return Ok(result);
        }
    }
}
