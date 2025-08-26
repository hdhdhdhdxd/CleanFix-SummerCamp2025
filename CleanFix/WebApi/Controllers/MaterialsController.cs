using Application.Materials.Queries.GetMaterial;
using Application.Materials.Queries.GetPaginatedMaterials;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/Materials/paginated
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<GetPaginatedMaterialDto>>> GetPaginatedMaterials([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _sender.Send(new GetPaginatedMaterialsQuery(pageNumber, pageSize));
            return Ok(result);
        }

        // GET: api/Materials/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetMaterialDto>> GetMaterial(int id)
        {
            var result = await _sender.Send(new GetMaterialQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
