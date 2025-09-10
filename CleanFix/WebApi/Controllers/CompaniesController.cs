using Application.Companies.Queries.GetCompany;
using Application.Companies.Queries.GetPaginatedCompanies;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly ISender _sender;

        public CompaniesController(ISender sender)
        {
            _sender = sender;
        }

        // GET: api/Companies/paginated
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<GetPaginatedCompanyDto>>> GetPaginatedCompanies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] int? typeIssueId = null)
        {
            Log.Information("GET api/companies/paginated called. PageNumber={PageNumber}, PageSize={PageSize}, TypeIssueId={TypeIssueId}", pageNumber, pageSize, typeIssueId);
            var result = await _sender.Send(new GetPaginatedCompaniesQuery(pageNumber, pageSize, typeIssueId));
            Log.Information("GET api/companies/paginated returned {Count} results.", result.Items.Count);
            return Ok(result);
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCompanyDto>> GetCompany(int id)
        {
            Log.Information("GET api/companies/{Id} called.", id);
            var result = await _sender.Send(new GetCompanyQuery(id));
            if (result == null)
            {
                Log.Warning("Company with id {Id} not found.", id);
                return NotFound();
            }
            Log.Information("Company with id {Id} returned successfully.", id);
            return Ok(result);
        }
    }
}
