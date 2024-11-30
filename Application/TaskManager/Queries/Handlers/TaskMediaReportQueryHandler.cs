using Applications.Interfaces.Repository;
using System.Net;
using Domain;
using MediatR;

namespace Applications.TaskManager.Queries.Handlers
{
    public class TaskMediaReportQueryHandler : IRequestHandler<TaskMediaReportQuery, Response>
    {
        private readonly IResponse _response;
        private readonly ITaskRepository _repository;

        public TaskMediaReportQueryHandler(IResponse response, ITaskRepository repository)
        {
            _response = response;
            _repository = repository;
        }

        public async Task<Response> Handle(TaskMediaReportQuery request, CancellationToken cancellationToken)
        {

            try
            {

                if (request.Profile != "gerente")
                    return await _response.CreateErrorResponseAsync(null, HttpStatusCode.Forbidden);
               

                var entity = await _repository.GetAllTaskClosed();

                var byUser = entity.GroupBy(x => x.LastModifiedBy).Select(g => new
                {
                    UserId = g.Key,
                    Media = g.Count()
                })
                .ToList();

                var mediaGeral = byUser.Average(x => x.Media);

                return await _response.CreateSuccessResponseAsync(mediaGeral, string.Empty);

            }
            catch (Exception ex)
            {
                return await _response.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError);
            }
        }
    }
}
