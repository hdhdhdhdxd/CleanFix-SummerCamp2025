using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.BaseDatos;
using WebApi.Entidades;
using WebApi.Interfaces;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/solicitations")]
    [ApiController]
    public class SolicitationsController : ControllerBase
    {
        private readonly ContextoBasedatos _context;

        public SolicitationsController(ContextoBasedatos context)
        {
            _context = context;
        }

        // GET: api/Solicitations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SolicitationDto>>> GetSolicitations()
        {
            List<SolicitationDto> listaSolicitations = new List<SolicitationDto>();

            // 1-Traer todos los distritos de la base de datos
            var solicitations = await _context.Solicitations
                            // Where(solicitation => solicitation.Id != Guid.Empty).
                            .OrderBy(solicitation => solicitation.Date)
                            .Take(10) // Limitar a 10 resultados
                            .ToListAsync();

            // 2-Devolver la lista de distritos en formato dto
             foreach (var solicitation in solicitations)
             {
                 listaSolicitations.Add(new SolicitationDto
                 {
                     Id = solicitation.Id,
                     Apartment = solicitation.Apartment,
                     Company = solicitation.Company,
                     Date = solicitation.Date,
                     Price = solicitation.Price,
                     Duration = solicitation.Duration,
                     Address = solicitation.Address,
                     Type = solicitation.Type,
                     Materials = solicitation.Materials,
                 });
             }

            return Ok (listaSolicitations);
        }

        // GET: api/Solicitations/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Solicitation>> GetSolicitation(Guid id)
        {
            var solicitation = await _context.Solicitations.FindAsync(id);

            if (solicitation == null)
            {
                return NotFound();
            }

            return solicitation;
        }

        // PUT: api/Solicitations/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSolicitation(Guid id, Solicitation solicitation)
        {
            if (id != solicitation.Id)
            {
                return BadRequest();
            }

            _context.Entry(solicitation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SolicitationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Solicitations
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Solicitation>> PostSolicitation(Solicitation solicitation)
        {
            _context.Solicitations.Add(solicitation);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitation", new { id = solicitation.Id }, solicitation);
        }

        // DELETE: api/Solicitations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSolicitation(Guid id)
        {
            var solicitation = await _context.Solicitations.FindAsync(id);
            if (solicitation == null)
            {
                return NotFound();
            }

            _context.Solicitations.Remove(solicitation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SolicitationExists(Guid id)
        {
            return _context.Solicitations.Any(e => e.Id == id);
        }
    }
}
