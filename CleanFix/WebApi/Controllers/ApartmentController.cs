using Application.Apartments.Queries.GetAparmets;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("api/apartamento")]
[ApiController]
public class ApartmentController : ControllerBase
{
    private readonly IGetApartmentsQuery _getApartmentsQuery;

    public ApartmentController(IGetApartmentsQuery getApartmentsQuery)
    {
        _getApartmentsQuery = getApartmentsQuery;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _getApartmentsQuery.ExecuteAsync();

        return Ok(result);
    }
}
