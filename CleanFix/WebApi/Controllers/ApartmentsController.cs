using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Apartments.Commands.DeleteAparment;
using Application.Apartments.Commands.PostApartments;
using Application.Apartments.Commands.PutApartments;
using Application.Apartments.Queries.GetApartments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.BaseDatos;
using WebApi.Entidades;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/apartments")]
    [ApiController]
    public class ApartmentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly DatabaseContext _context;

        public ApartmentsController(IMediator mediator, DatabaseContext context)
        {
            _mediator = mediator;
            _context = context;
        }

        // GET: api/Apartments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetApartmentsDto>>> GetApartments()
        {
            var result = await _mediator.Send(new GetApartmentsQuery());

            return Ok(result);
        }

        // GET: api/Apartments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Apartment>> GetApartment(Guid id)
        {
            var apartment = await _context.Apartments.FindAsync(id);

            if (apartment == null)
            {
                return NotFound();
            }

            return apartment;
        }

        // PUT: api/Apartments/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApartment(Guid id, Apartment apartment)
        {
            await _mediator.Send(new PutApartmentQuery());

            return NoContent();
        }

        // POST: api/Apartments
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Apartment>> PostApartment(Apartment apartment)
        {
            await _mediator.Send(new PostApartmentsQuery());

            return CreatedAtAction("GetApartment", new { id = apartment.Id }, apartment);
        }

        // DELETE: api/Apartments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApartment(Guid id)
        {
            await _mediator.Send(new DeleteApartmentsQuery());

            return NoContent();
        }

        private bool ApartmentExists(Guid id)
        {
            return _context.Apartments.Any(e => e.Id == id);
        }
    }
}
