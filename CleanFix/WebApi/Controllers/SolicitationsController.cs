using Application.Solicitations.Queries.GetPaginatedSolicitations;
using Application.Solicitations.Queries.GetSolicitations;
using Application.Solicitations.Queries.GetSolicitation;
using Application.Solicitations.Commands.CreateSolicitation;
using Application.Solicitations.Commands.UpdateSolicitation;
using Application.Solicitations.Commands.DeleteSolicitation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/solicitations")]
    [ApiController]
    public class SolicitationsController : ControllerBase
    {
        private readonly ISender _sender;

        public SolicitationsController(ISender sender)
        {
            _sender = sender;
        }

        // GET: api/Solicitations/paginated
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<GetPaginatedSolicitationDto>>> GetPaginatedSolicitations([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _sender.Send(new GetPaginatedSolicitationsQuery(pageNumber, pageSize));
            return Ok(result);
        }

        // GET: api/Solicitations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetSolicitationsDto>>> GetSolicitations()
        {
            var result = await _sender.Send(new GetSolicitationsQuery());
            return Ok(result);
        }

        // GET: api/Solicitations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetSolicitationDto>> GetSolicitation(int id)
        {
            var result = await _sender.Send(new GetSolicitationQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // POST: api/Solicitations
        [HttpPost]
        public async Task<ActionResult<int>> PostSolicitation([FromBody] CreateSolicitationDto solicitationDto)
        {
            var command = new CreateSolicitationCommand { Solicitation = solicitationDto };
            var newSolicitationId = await _sender.Send(command);
            return CreatedAtAction(
                nameof(GetSolicitation),
                new { id = newSolicitationId },
                newSolicitationId
            );
        }

        // PUT: api/Solicitations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitation(int id, [FromBody] UpdateSolicitationDto solicitationDto)
        {
            if (solicitationDto.Id != default && solicitationDto.Id != id)
                return BadRequest("El id de la ruta y el del cuerpo no coinciden.");
            solicitationDto.Id = id;
            var command = new UpdateSolicitationCommand { Solicitation = solicitationDto };
            await _sender.Send(command);
            return NoContent();
        }

        // DELETE: api/Solicitations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitation(int id)
        {
            var command = new DeleteSolicitationCommand(id);
            var result = await _sender.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
