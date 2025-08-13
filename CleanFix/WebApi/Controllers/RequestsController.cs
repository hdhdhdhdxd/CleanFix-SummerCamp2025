using Application.Requests.Queries.GetPaginatedRequests;
using Application.Requests.Queries.GetRequests;
using Application.Requests.Queries.GetRequest;
using Application.Requests.Commands.CreateRequest;
using Application.Requests.Commands.UpdateRequest;
using Application.Requests.Commands.DeleteRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/requests")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly ISender _sender;

        public RequestsController(ISender sender)
        {
            _sender = sender;
        }

        // GET: api/Requests/paginated
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<GetPaginatedRequestDto>>> GetPaginatedRequests([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _sender.Send(new GetPaginatedRequestsQuery(pageNumber, pageSize));
            return Ok(result);
        }

        // GET: api/Requests
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetRequestsDto>>> GetRequests()
        {
            var result = await _sender.Send(new GetRequestsQuery());
            return Ok(result);
        }

        // GET: api/Requests/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetRequestDto>> GetRequest(int id)
        {
            var result = await _sender.Send(new GetRequestQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // POST: api/Requests
        [HttpPost]
        public async Task<ActionResult<int>> PostRequest([FromBody] CreateRequestDto requestDto)
        {
            var command = new CreateRequestCommand { Request = requestDto };
            var newRequestId = await _sender.Send(command);
            return CreatedAtAction(
                nameof(GetRequest),
                new { id = newRequestId },
                newRequestId
            );
        }

        // PUT: api/Requests/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, [FromBody] UpdateRequestDto requestDto)
        {
            if (requestDto.Id != default && requestDto.Id != id)
                return BadRequest("El id de la ruta y el del cuerpo no coinciden.");
            requestDto.Id = id;
            var command = new UpdateRequestCommand { Request = requestDto };
            await _sender.Send(command);
            return NoContent();
        }

        // DELETE: api/Requests/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            var command = new DeleteRequestCommand(id);
            var result = await _sender.Send(command);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
