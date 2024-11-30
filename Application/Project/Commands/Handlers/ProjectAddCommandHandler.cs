using System.Net;
using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Domain;
using MediatR;

namespace Applications.Project.Commands.Handlers
{
    public class ProjectAddCommandHandler : IRequestHandler<ProjectAddCommand, Response>
    {
        private readonly IResponse _response;
        private readonly IProjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskManagerMappers _taskManagerMappers;


        public ProjectAddCommandHandler(IResponse response,  IProjectRepository repository, IUnitOfWork unitOfWork, ITaskManagerMappers taskManagerMappers)
        {
            _response = response;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _taskManagerMappers = taskManagerMappers;
        }

        public async Task<Response> Handle(ProjectAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Description) || string.IsNullOrEmpty(request.Name))
                    return await _response.CreateErrorResponseAsync("Name or description be empty", HttpStatusCode.BadRequest);

                var entity = _taskManagerMappers.Map(request);

                await _repository.AddAsync(entity);

                await _unitOfWork.CompleteAsync(cancellationToken);

                return await _response.CreateSuccessResponseAsync(null, string.Empty);

            }
            catch (Exception)
            {
                return await _response.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError);
            }
        }
    }
}
