using Application.Materials.Queries.GetPaginatedMaterials;
using Application.Materials.Queries.GetMaterials;
using Application.Materials.Queries.GetMaterial;
using Application.Materials.Commands.CreateMaterial;
using Application.Materials.Commands.UpdateMaterial;
using Application.Materials.Commands.DeleteMaterial;
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

        // GET: api/Materials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMaterialsDto>>> GetMaterials()
        {
            var result = await _sender.Send(new GetMaterialsQuery());
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

        // POST: api/Materials
        [HttpPost]
        public async Task<ActionResult<int>> PostMaterial([FromBody] CreateMaterialDto materialDto)
        {
            var command = new CreateMaterialCommand { Material = materialDto };
            var newMaterialId = await _sender.Send(command);
            return CreatedAtAction(
                nameof(GetMaterial),
                new { id = newMaterialId },
                newMaterialId
            );
        }

        // PUT: api/Materials/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial(int id, [FromBody] UpdateMaterialDto materialDto)
        {
            if (materialDto.Id != default && materialDto.Id != id)
                return BadRequest("El id de la ruta y el del cuerpo no coinciden.");
            materialDto.Id = id;
            var command = new UpdateMaterialCommand { Material = materialDto };
            await _sender.Send(command);
            return NoContent();
        }

        // DELETE: api/Materials/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var command = new DeleteMaterialCommand(id);
            var result = await _sender.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
