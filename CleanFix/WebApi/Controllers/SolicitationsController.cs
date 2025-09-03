using Application.Solicitations.Commands.CreateSolicitation;
using Application.Solicitations.Commands.DeleteSolicitation;
using Application.Solicitations.Queries.GetPaginatedSolicitations;
using Application.Solicitations.Queries.GetSolicitation;
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

        // GET: api/solicitations/paginated
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<GetPaginatedSolicitationDto>>> GetPaginatedSolicitations(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterString = null)
        {
            var result = await _sender.Send(new GetPaginatedSolicitationsQuery(pageNumber, pageSize, filterString));
            return Ok(result);
        }

        // GET: api/solicitations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetSolicitationDto>> GetSolicitation(int id)
        {
            var result = await _sender.Send(new GetSolicitationQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // POST: api/solicitations
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



        // DELETE: api/solicitations/{id}
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
