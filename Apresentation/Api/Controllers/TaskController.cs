using System.Net;
using Applications.Project.Commands;
using Applications.Project.Queries;
using Applications.TaskManager.Commands;
using Applications.TaskManager.Queries;
using Applications.TaskManager.Queries.Handlers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/taskmanager/task")]

    public class TaskController : Controller
    {
        private readonly IMediator _mediator;


        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] TaskAddCommand request)
        {
            return Ok(await _mediator.Send(request));
        }


        [HttpPut()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put([FromBody] TaskUpdateCommand request, [FromQuery] Guid Id)
        {
            request.SetId(Id);

            return Ok(await _mediator.Send(request));
        }


        [HttpGet("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid Id)
        {
            var query = new TaskGetQuery() { Id = Id };

            return Ok(await _mediator.Send(query));
        }


        [HttpDelete("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            var query = new TaskDeleteCommand() { Id = Id };

            return Ok(await _mediator.Send(query));
        }


        [HttpGet()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] TaskListQuery request)
        {

            return Ok(await _mediator.Send(request));
        }

        [HttpPost("comment")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PostComment([FromBody] TaskCommentAddCommand request)
        {
            return Ok(await _mediator.Send(request));
        }


        [HttpPost("MediaReport")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetMediaReport([FromBody] TaskMediaReportQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

    }
}
