using Application.Apartments.Queries.GetPaginatedApartment;
using Application.Companies.Commands.CreateCompany;
using Application.Companies.Commands.DeleteCompany;
using Application.Companies.Commands.UpdateCompany;
using Application.Companies.Queries.GetCompanies;
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

        // GET: api/Companies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCompaniesDto>>> GetCompanies()
        {
            var result = await _sender.Send(new GetCompaniesQuery());
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

        // PUT: api/Companies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompany(int id, [FromBody] UpdateCompanyDto companyDto)
        {
            if (companyDto.Id != 0 && companyDto.Id != id)
                return BadRequest("El id de la ruta y el del cuerpo no coinciden.");
            companyDto.Id = id;
            var command = new UpdateCompanyCommand
            {
                Company = companyDto
            };
            await _sender.Send(command);
            return NoContent();
        }

        // POST: api/Companies
        [HttpPost]
        public async Task<ActionResult<GetCompaniesDto>> PostCompany([FromBody] CreateCompanyDto companyDto)
        {
            var command = new CreateCompanyCommand
            {
                Company = companyDto
            };
            var newCompanyId = await _sender.Send(command);
            var createdCompany = await _sender.Send(new GetCompanyQuery(newCompanyId));
            return CreatedAtAction(
                nameof(GetCompany),
                new { id = newCompanyId },
                createdCompany
            );
        }

        // DELETE: api/Companies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var command = new DeleteCompanyCommand(id);
            var result = await _sender.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
