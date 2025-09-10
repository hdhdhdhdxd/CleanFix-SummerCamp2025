using Application.Solicitations.Commands.CreateSolicitation;
using Application.Solicitations.Commands.DeleteSolicitation;
using Application.Solicitations.Queries.GetPaginatedSolicitations;
using Application.Solicitations.Queries.GetSolicitation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

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
        [Authorize]
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<GetPaginatedSolicitationDto>>> GetPaginatedSolicitations(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterString = null)
        {
            Log.Information("GET api/solicitations/paginated called. PageNumber={PageNumber}, PageSize={PageSize}, Filter={Filter}", pageNumber, pageSize, filterString);
            var result = await _sender.Send(new GetPaginatedSolicitationsQuery(pageNumber, pageSize, filterString));
            Log.Information("GET api/solicitations/paginated returned {Count} results (TotalCount={TotalCount}).", result.Items.Count, result.TotalCount);
            return Ok(result);
        }

        // GET: api/solicitations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetSolicitationDto>> GetSolicitation(int id)
        {
            Log.Information("GET api/solicitations/{Id} called.", id);
            var result = await _sender.Send(new GetSolicitationQuery(id));
            if (result == null)
            {
                Log.Warning("Solicitation with id {Id} not found.", id);
                return NotFound();
            }
            Log.Information("Solicitation with id {Id} returned successfully.", id);
            return Ok(result);
        }

        // POST: api/solicitations
        [HttpPost]
        public async Task<ActionResult<int>> PostSolicitation([FromBody] CreateSolicitationDto solicitationDto)
        {
            Log.Information("POST api/solicitations called. Description={Description}, Address={Address}, IssueTypeId={IssueTypeId}, BuildingCode={BuildingCode}, ApartmentAmount={ApartmentAmount}", solicitationDto.Description, solicitationDto.Address, solicitationDto.IssueTypeId, solicitationDto.BuildingCode, solicitationDto.ApartmentAmount);
            var command = new CreateSolicitationCommand { Solicitation = solicitationDto };
            var newSolicitationId = await _sender.Send(command);
            Log.Information("POST api/solicitations created Solicitation with id {Id}.", newSolicitationId);
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
            Log.Information("DELETE api/solicitations/{Id} called.", id);
            var command = new DeleteSolicitationCommand(id);
            var result = await _sender.Send(command);
            if (!result)
            {
                Log.Warning("Solicitation with id {Id} not found for deletion.", id);
                return NotFound();
            }
            Log.Information("Solicitation with id {Id} deleted successfully.", id);
            return NoContent();
        }
    }
}
