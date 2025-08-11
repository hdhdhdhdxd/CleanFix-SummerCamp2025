using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Materials.Commands.CreateMaterial;
using Application.Materials.Commands.DeleteMaterial;
using Application.Materials.Commands.UpdateMaterial;
using Application.Materials.Queries.GetMaterial;
using Application.Materials.Queries.GetMaterials;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/materials")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly ISender _sender;

        public MaterialsController(IMediator sender)
        {
            _sender = sender;
        }

        // GET: api/Materials
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMaterialsDto>>> GetMaterials()
        {
            var result = await _sender.Send(new GetMaterialsQuery());
            return Ok(result);
        }

        // GET: api/Materials/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetMaterialDto>> GetMaterial(Guid id)
        {
            var result = await _sender.Send(new GetMaterialQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // PUT: api/Materials/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMaterial(Guid id, [FromBody] UpdateMaterialDto materialDto)
        {
            if (materialDto.Id != Guid.Empty && materialDto.Id != id)
                return BadRequest("El id de la ruta y el del cuerpo no coinciden.");
            materialDto.Id = id;
            var command = new UpdateMaterialCommand
            {
                Material = materialDto
            };
            await _sender.Send(command);
            return NoContent();
        }

        // POST: api/Materials
        [HttpPost]
        public async Task<ActionResult<GetMaterialsDto>> PostMaterial([FromBody] CreateMaterialDto materialDto)
        {
            var command = new CreateMaterialCommand
            {
                Material = materialDto
            };
            var newMaterialId = await _sender.Send(command);
            var createdMaterial = await _sender.Send(new GetMaterialQuery(newMaterialId));
            return CreatedAtAction(
                nameof(GetMaterial),
                new { id = newMaterialId },
                createdMaterial
            );
        }

        // DELETE: api/Materials/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(Guid id)
        {
            var command = new DeleteMaterialCommand(id);
            var result = await _sender.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
