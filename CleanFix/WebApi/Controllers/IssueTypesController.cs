using Domain.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

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
        Log.Information("GET api/issuetypes called.");
        var result = await _context.IssueTypes.ToListAsync();
        Log.Information("GET api/issuetypes returned {Count} results.", result.Count);
        return result;
    }
}
