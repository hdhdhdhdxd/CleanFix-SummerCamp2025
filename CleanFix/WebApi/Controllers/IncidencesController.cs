using Application.Incidences.Commands.CreateIncidence;
using Application.Incidences.Commands.DeleteIncidence;
using Application.Incidences.Commands.UpdateIncidence;
using Application.Incidences.Queries.GetIncidence;
using Application.Incidences.Queries.GetPaginatedIncidences;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/incidences")]
    [ApiController]
    public class IncidencesController : ControllerBase
    {
        private readonly ISender _sender;

        public IncidencesController(ISender sender)
        {
            _sender = sender;
        }

        // GET: api/Incidences/paginated
        [HttpGet("paginated")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<GetPaginatedIncidenceDto>>> GetPaginatedIncidences(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterString = null)
        {
            var result = await _sender.Send(new GetPaginatedIncidencesQuery(pageNumber, pageSize, filterString));
            return Ok(result);
        }

        // GET: api/Incidences/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetIncidenceDto>> GetIncidence(int id)
        {
            var result = await _sender.Send(new GetIncidenceQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // POST: api/Incidences
        [HttpPost]
        public async Task<ActionResult<int>> PostIncidence([FromBody] CreateIncidenceDto incidenceDto)
        {
            var command = new CreateIncidenceCommand { Incidence = incidenceDto };
            var newIncidenceId = await _sender.Send(command);
            return CreatedAtAction(
                nameof(GetIncidence),
                new { id = newIncidenceId },
                newIncidenceId
            );
        }

        // PUT: api/Incidences/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncidence(int id, [FromBody] UpdateIncidenceDto incidenceDto)
        {
            if (incidenceDto.Id != default && incidenceDto.Id != id)
                return BadRequest("El id de la ruta y el del cuerpo no coinciden.");
            incidenceDto.Id = id;
            var command = new UpdateIncidenceCommand { Incidence = incidenceDto };
            try
            {
                await _sender.Send(command);
                return NoContent();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Conflicto de concurrencia"))
                {
                    return Conflict(new ProblemDetails
                    {
                        Title = "Conflicto de concurrencia",
                        Detail = "La incidencia ha sido modificada por otro usuario"
                    });
                }
                throw;
            }
        }

        // DELETE: api/Incidences/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncidence(int id)
        {
            var command = new DeleteIncidenceCommand(id);
            var result = await _sender.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
