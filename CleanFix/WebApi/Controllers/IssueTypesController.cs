using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.BaseDatos;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IssueTypesController : ControllerBase
{
    private readonly DatabaseContext _context;

    public IssueTypesController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IssueType>>> GetAll()
    {
        return await _context.IssueTypes.ToListAsync();
    }
}
