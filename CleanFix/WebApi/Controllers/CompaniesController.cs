using Application.Companies.Queries.GetCompany;
using Application.Companies.Queries.GetPaginatedCompanies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<GetPaginatedCompanyDto>>> GetPaginatedCompanies([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _sender.Send(new GetPaginatedCompaniesQuery(pageNumber, pageSize));

            return Ok(result);
        }

        // GET: api/Companies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCompanyDto>> GetCompany(int id)
        {
            var result = await _sender.Send(new GetCompanyQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }
    }
}
