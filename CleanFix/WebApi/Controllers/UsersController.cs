using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.BaseDatos;
using WebApi.Entidades;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            List<UserDto> listaUsers = new List<UserDto>();

            var users = await _context.Users
                            // Where(user => user.Id != Guid.Empty).
                            .OrderBy(user => user.Name)
                            .Take(10) // Limitar a 10 resultados
                            .ToListAsync();
           
                // Reemplaza la asignación directa de Apartment por una conversión explícita a ApartmentDto
                foreach (var user in users)
                {
                    listaUsers.Add(new UserDto
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Applications = user.Applications,
                        Apartment = user.Apartment == null ? null : new ApartmentDto
                        {
                            Id = user.Apartment.Id,
                            FloorNumber = user.Apartment.FloorNumber,
                            Address = user.Apartment.Address,
                            Surface = user.Apartment.Surface,
                            RoomNumber = user.Apartment.RoomNumber,
                            BathroomNumber = user.Apartment.BathroomNumber
                        }
                    });
                }
                 
            

            return Ok (listaUsers);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
