using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Applications.Abstracts;
using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Applications.Project.Model;
using Domain;
using Domain.Domain;
using MediatR;

namespace Applications.Project.Queries.Handlers
{
    public class ProjectListQueryHandler : IRequestHandler<ProjectListQuery, Response>
    {
        private readonly IResponse _response;
        private readonly IProjectRepository _repository;
        private readonly ITaskManagerMappers _taskMangerMappers;

        public ProjectListQueryHandler(IResponse response, IProjectRepository metaRepository, ITaskManagerMappers taskManagerMappers)
        {
            _response = response;
            _repository = metaRepository;
            _taskMangerMappers = taskManagerMappers;
        }

        public async Task<Response> Handle(ProjectListQuery request, CancellationToken cancellationToken)
        {

            try
            {
                var filter = await _repository.Filter(request.Name, request.Deleted, request.PageSize.Value, request.Page.Value);

                var listModel = _taskMangerMappers.Map(filter.entities);


                var paged = new Paged<ProjectModel>
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

