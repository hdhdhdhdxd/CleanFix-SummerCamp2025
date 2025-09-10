using Application.Apartments.Commands.CreateApartment;
using Application.Apartments.Commands.DeleteApartment;
using Application.Apartments.Commands.UpdateApartment;
using Application.Apartments.Queries.GetApartment;
using Application.Apartments.Queries.GetPaginatedApartment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly ISender _sender;

        public ApartmentsController(ISender sender)
        {
            _sender = sender;
        }


        // GET: api/Apartments/paginated
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<GetPaginatedApartmentDto>>> GetPaginatedApartments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            Log.Information("GET api/apartments/paginated called. PageNumber={PageNumber}, PageSize={PageSize}", pageNumber, pageSize);
            var result = await _sender.Send(new GetPaginatedApartmentsQuery(pageNumber, pageSize));
            Log.Information("GET api/apartments/paginated returned {Count} results.", result.Items.Count);
            return Ok(result);
        }

        // GET: api/Apartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetApartmentDto>> GetApartment(int id)
        {
            Log.Information("GET api/apartments/{Id} called.", id);
            var result = await _sender.Send(new GetApartmentQuery(id));
            if (result == null)
            {
                Log.Warning("Apartment with id {Id} not found.", id);
                return NotFound();
            }
            Log.Information("Apartment with id {Id} returned successfully.", id);
            return Ok(result);
        }

        // PUT: api/Apartments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartment(int id, [FromBody] UpdateApartmentDto apartmentDto)
        {
            Log.Information("PUT api/apartments/{Id} called.", id);
            if (apartmentDto.Id != 0 && apartmentDto.Id != id)
            {
                Log.Warning("PUT api/apartments/{Id} failed: route id and body id do not match.", id);
                return BadRequest("El id de la ruta y el del cuerpo no coinciden.");
            }
            apartmentDto.Id = id;
            var command = new UpdateApartmentCommand
            {
                Apartment = apartmentDto
            };
            await _sender.Send(command);
            Log.Information("Apartment with id {Id} updated successfully.", id);
            return NoContent();
        }

        // POST: api/Apartments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetPaginatedApartmentDto>> PostApartment([FromBody] CreateApartmentDto apartmentDto)
        {
            var command = new CreateApartmentCommand
            {
                Apartment = apartmentDto
            };
            var newApartmentId = await _sender.Send(command);
            Log.Information("POST api/apartments created Apartment with id {Id}.", newApartmentId);
            var createdApartment = await _sender.Send(new GetApartmentQuery(newApartmentId));
            return CreatedAtAction(
                nameof(GetApartment),
                new { id = newApartmentId },
                createdApartment
            );
        }

        // DELETE: api/Apartments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(int id)
        {
            Log.Information("DELETE api/apartments/{Id} called.", id);
            var command = new DeleteApartmentCommand(id);
            var result = await _sender.Send(command);
            if (!result)
            {
                Log.Warning("Apartment with id {Id} not found for deletion.", id);
                return NotFound();
            }
            Log.Information("Apartment with id {Id} deleted successfully.", id);
            return NoContent();
        }
    }
}
