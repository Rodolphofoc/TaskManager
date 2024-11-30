using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Applications.Project.Commands;
using Domain;
using MediatR;
using System.Net;

namespace Applications.TaskManager.Commands.Handlers
{
    public class TaskAddCommandHandler : IRequestHandler<TaskAddCommand, Response>
    {
        private readonly IResponse _response;
        private readonly ITaskRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskManagerMappers _taskManagerMappers;
        private readonly IProjectRepository _projectRepository;


        public TaskAddCommandHandler(IResponse response, ITaskRepository repository, IUnitOfWork unitOfWork, ITaskManagerMappers taskManagerMappers, IProjectRepository projectRepository)
        {
            _response = response;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _taskManagerMappers = taskManagerMappers;
            _projectRepository = projectRepository;
        }

        public async Task<Response> Handle(TaskAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Description) || string.IsNullOrEmpty(request.Title))
                    return await _response.CreateErrorResponseAsync("Name or description be empty", HttpStatusCode.BadRequest);

                var project = await _projectRepository.GetByIdAsync(request.ProjectId);

                if (project is null)
                    return await _response.CreateErrorResponseAsync("Project not exist", HttpStatusCode.BadRequest);

                if (project.Tasks.Count == 20)
                    return await _response.CreateErrorResponseAsync("Task limit reached", HttpStatusCode.BadRequest);


                var entity = _taskManagerMappers.Map(request);

                entity.Project = project;
                entity.ProjectId = project.Id;
                entity.LastModifiedBy = request.User;

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
