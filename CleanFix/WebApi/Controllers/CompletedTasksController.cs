using Application.CompletedTasks.Commands.CreateCompletedTask;
using Application.CompletedTasks.Queries.GetCompletedTask;
using Application.CompletedTasks.Queries.GetPaginatedCompletedTasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace WebApi.Controllers
{
    [Route("api/completedtasks")]
    [ApiController]
    public class CompletedTasksController : ControllerBase
    {
        private readonly ISender _sender;

        public CompletedTasksController(ISender sender)
        {
            _sender = sender;
        }

        // GET: api/completedtasks/paginated
        [HttpGet("paginated")]
        public async Task<ActionResult<IEnumerable<GetPaginatedCompletedTaskDto>>> GetPaginatedCompletedTasks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? filterString = null)
        {
            Log.Information("GET api/completedtasks/paginated called. PageNumber={PageNumber}, PageSize={PageSize}, Filter={Filter}", pageNumber, pageSize, filterString);
            var result = await _sender.Send(new GetPaginatedCompletedTasksQuery(pageNumber, pageSize, filterString));
            Log.Information("GET api/completedtasks/paginated returned {Count} results.", result.Items.Count);
            return Ok(result);
        }

        // GET: api/completedtasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCompletedTaskDto>> GetCompletedTask(int id)
        {
            Log.Information("GET api/completedtasks/{Id} called.", id);
            var result = await _sender.Send(new GetCompletedTaskQuery(id));
            if (result == null)
            {
                Log.Warning("CompletedTask with id {Id} not found.", id);
                return NotFound();
            }
            Log.Information("CompletedTask with id {Id} returned successfully.", id);
            return Ok(result);
        }

        // POST: api/completedtasks
        [HttpPost]
        public async Task<ActionResult<int>> PostCompletedTask([FromBody] CreateCompletedTaskDto completedTaskDto)
        {
            Log.Information("POST api/completedtasks called.");
            var command = new CreateCompletedTaskCommand { CompletedTask = completedTaskDto };
            var newCompletedTaskId = await _sender.Send(command);
            Log.Information("POST api/completedtasks created CompletedTask with id {Id}.", newCompletedTaskId);
            return CreatedAtAction(
                nameof(GetCompletedTask),
                new { id = newCompletedTaskId },
                newCompletedTaskId
            );
        }
    }
}
