using Application.CompletedTasks.Commands.CreateCompletedTask;
using Application.CompletedTasks.Queries.GetCompletedTask;
using Application.CompletedTasks.Queries.GetCompletedTasks;
using Application.CompletedTasks.Queries.GetPaginatedCompletedTasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<GetPaginatedCompletedTaskDto>>> GetPaginatedCompletedTasks([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _sender.Send(new GetPaginatedCompletedTasksQuery(pageNumber, pageSize));
            return Ok(result);
        }

        // GET: api/completedtasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCompletedTasksDto>>> GetCompletedTasks()
        {
            var result = await _sender.Send(new GetCompletedTasksQuery());
            return Ok(result);
        }

        // GET: api/completedtasks/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<GetCompletedTaskDto>> GetCompletedTask(int id)
        {
            var result = await _sender.Send(new GetCompletedTaskQuery(id));
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        // POST: api/completedtasks
        [HttpPost]
        public async Task<ActionResult<int>> PostCompletedTask([FromBody] CreateCompletedTaskDto completedTaskDto)
        {
            var command = new CreateCompletedTaskCommand { CompletedTask = completedTaskDto };
            var newCompletedTaskId = await _sender.Send(command);
            return CreatedAtAction(
                nameof(GetCompletedTask),
                new { id = newCompletedTaskId },
                newCompletedTaskId
            );
        }
    }
}
