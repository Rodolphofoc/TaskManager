using System.Net;
using Applications.Abstracts;
using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Applications.TaskManager.Model;
using Domain;
using MediatR;

namespace Applications.TaskManager.Queries.Handlers
{
    internal class TaskListQueryHandler : IRequestHandler<TaskListQuery, Response>
    {
        private readonly IResponse _response;
        private readonly ITaskRepository _repository;
        private readonly ITaskManagerMappers _taskMangerMappers;

        public TaskListQueryHandler(IResponse response, ITaskRepository repository, ITaskManagerMappers taskManagerMappers)
        {
            _response = response;
            _repository = repository;
            _taskMangerMappers = taskManagerMappers;
        }

        public async Task<Response> Handle(TaskListQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var filter = await _repository.Filter(request.Name, request.Priority, request.Deleted, request.PageSize.Value, request.Page.Value, request.ProjectId);

                var listModel = _taskMangerMappers.Map(filter.entities);


                var paged = new Paged<TaskModel>
                {
                    CurrentPage = request.Page.Value,
                    PageSize = request.PageSize.Value,
                    Records = listModel,
                    RecordsInPage = filter.entities.Count,
                    TotalRecords = filter.totalRecords,
                };


                return await _response.CreateSuccessResponseAsync(paged, string.Empty);
            }
            catch (Exception ex)
            {
                return await _response.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError);
            }
        }
    }
}
