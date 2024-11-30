using System.Net;
using Applications.Interfaces.Repository;
using Domain;
using MediatR;

namespace Applications.TaskManager.Queries.Handlers
{
    public class TaskGetQueryHandler : IRequestHandler<TaskGetQuery, Response>
    {
        private readonly IResponse _response;
        private readonly ITaskRepository _repository;

        public TaskGetQueryHandler(IResponse response, ITaskRepository repository)
        {
            _response = response;
            _repository = repository;
        }

        public async Task<Response> Handle(TaskGetQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var entity = _repository.FindById(request.Id);

                if (entity == null || entity.Deleted)
                    return await _response.CreateErrorResponseAsync(null, HttpStatusCode.NotFound);

                return await _response.CreateSuccessResponseAsync(entity, string.Empty);

            }
            catch (Exception ex)
            {
                return await _response.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError);
            }
        }
    }
}
