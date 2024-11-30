using System.Net;
using Applications.Project.Commands;
using Applications.Project.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/taskmanager/project")]

    public class ProjectController : Controller
    {
        private readonly IMediator _mediator;


        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody]ProjectAddCommand request)
        {
            return Ok(await _mediator.Send(request));
        }


        [HttpPut()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Put([FromBody]ProjectUpdateCommand request, [FromQuery] Guid Id)
        {
            request.SetId(Id);

            return Ok(await _mediator.Send(request));
        }


        [HttpGet("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Get([FromRoute] Guid Id)
        {
            var query = new ProjectGetQuery() { Id = Id };

            return Ok(await _mediator.Send(query));
        }


        [HttpDelete("{Id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] Guid Id)
        {
            var query = new ProjectDeleteCommand() { Id = Id };

            return Ok(await _mediator.Send(query));
        }


        [HttpGet()]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAll([FromQuery] ProjectListQuery request)
        {

            return Ok(await _mediator.Send(request));
        }

    }
}
