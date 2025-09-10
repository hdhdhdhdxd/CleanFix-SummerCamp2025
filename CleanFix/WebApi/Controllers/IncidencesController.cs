using Application.Incidences.Commands.CreateIncidence;
using Application.Incidences.Commands.DeleteIncidence;
using Application.Incidences.Commands.UpdateIncidence;
using Application.Incidences.Queries.GetIncidence;
using Application.Incidences.Queries.GetPaginatedIncidences;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
        [Authorize]
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<GetPaginatedIncidenceDto>>> GetPaginatedIncidences(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterString = null)
        {
            Log.Information("GET api/incidences/paginated called. PageNumber={PageNumber}, PageSize={PageSize}, Filter={Filter}", pageNumber, pageSize, filterString);
            var result = await _sender.Send(new GetPaginatedIncidencesQuery(pageNumber, pageSize, filterString));
            Log.Information("GET api/incidences/paginated returned {Count} results.", result.Items.Count);
            return Ok(result);
        }

        // GET: api/Incidences/{id}
        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetIncidenceDto>> GetIncidence(int id)
        {
            Log.Information("GET api/incidences/{Id} called.", id);
            var result = await _sender.Send(new GetIncidenceQuery(id));
            if (result == null)
            {
                Log.Warning("Incidence with id {Id} not found.", id);
                return NotFound();
            }
            Log.Information("Incidence with id {Id} returned successfully.", id);
            return Ok(result);
        }

        // POST: api/Incidences
        [HttpPost]
        public async Task<ActionResult<int>> PostIncidence([FromBody] CreateIncidenceDto incidenceDto)
        {
            Log.Information("POST api/incidences called.");
            var command = new CreateIncidenceCommand { Incidence = incidenceDto };
            var newIncidenceId = await _sender.Send(command);
            Log.Information("POST api/incidences created Incidence with id {Id}.", newIncidenceId);
            return CreatedAtAction(
                nameof(GetIncidence),
                new { id = newIncidenceId },
                newIncidenceId
            );
        }

        // PUT: api/Incidences/{id}
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutIncidence(int id, [FromBody] UpdateIncidenceDto incidenceDto)
        {
            Log.Information("PUT api/incidences/{Id} called.", id);
            if (incidenceDto.Id != default && incidenceDto.Id != id)
            {
                Log.Warning("PUT api/incidences/{Id} failed: route id and body id do not match.", id);
                return BadRequest("El id de la ruta y el del cuerpo no coinciden.");
            }
            incidenceDto.Id = id;
            var command = new UpdateIncidenceCommand { Incidence = incidenceDto };
            try
            {
                await _sender.Send(command);
                Log.Information("Incidence with id {Id} updated successfully.", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "PUT api/incidences/{Id} failed due to exception.", id);
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
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteIncidence(int id)
        {
            Log.Information("DELETE api/incidences/{Id} called.", id);
            var command = new DeleteIncidenceCommand(id);
            var result = await _sender.Send(command);
            if (!result)
            {
                Log.Warning("Incidence with id {Id} not found for deletion.", id);
                return NotFound();
            }
            Log.Information("Incidence with id {Id} deleted successfully.", id);
            return NoContent();
        }
    }
}
