using Application.Apartments.Commands.CreateApartment;
using Application.Apartments.Commands.DeleteApartment;
using Application.Apartments.Commands.UpdateApartment;
using Application.Apartments.Queries.GetApartment;
using Application.Apartments.Queries.GetApartments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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

        // GET: api/Apartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetApartmentsDto>>> GetApartments()
        {
            var result = await _sender.Send(new GetApartmentsQuery());

            return Ok(result);
        }

        // GET: api/Apartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetApartmentDto>> GetApartment(Guid id)
        {
            var result = await _sender.Send(new GetApartmentQuery(id));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // PUT: api/Apartments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartment(Guid id, [FromBody] UpdateApartmentDto apartmentDto)
        {
            if (apartmentDto.Id != Guid.Empty && apartmentDto.Id != id)
                return BadRequest("El id de la ruta y el del cuerpo no coinciden.");

            apartmentDto.Id = id;

            var command = new UpdateApartmentCommand
            {
                Apartment = apartmentDto
            };

            await _sender.Send(command);

            return NoContent();
        }

        // POST: api/Apartments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GetApartmentsDto>> PostApartment([FromBody] CreateApartmentDto apartmentDto)
        {
            var command = new CreateApartmentCommand
            {
                Apartment = apartmentDto
            };

            var newApartmentId = await _sender.Send(command);

            var createdApartment = await _sender.Send(new GetApartmentQuery(newApartmentId));

            return CreatedAtAction(
                nameof(GetApartment),
                new { id = newApartmentId },
                createdApartment
            );
        }

        // DELETE: api/Apartments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(Guid id)
        {
            var command = new DeleteApartmentCommand(id);

            var result = await _sender.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
