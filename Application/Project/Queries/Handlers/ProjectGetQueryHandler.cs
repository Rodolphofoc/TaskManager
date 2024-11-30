using System.Net;
using Applications.Interfaces.Repository;
using Domain;
using MediatR;

namespace Applications.Project.Queries.Handlers
{
    public  class ProjectGetQueryHandler : IRequestHandler<ProjectGetQuery, Response>
    {
        private readonly IResponse _response;
        private readonly IProjectRepository _repository;

        public ProjectGetQueryHandler(IResponse response, IProjectRepository metaRepository)
        {
            _response = response;
            _repository = metaRepository;
        }

        public async Task<Response> Handle(ProjectGetQuery request, CancellationToken cancellationToken)
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
