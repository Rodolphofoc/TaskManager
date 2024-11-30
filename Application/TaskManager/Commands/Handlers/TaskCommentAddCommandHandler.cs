using System.Net;
using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Domain;
using MediatR;

namespace Applications.TaskManager.Commands.Handlers
{
    public class TaskCommentAddCommandHandler : IRequestHandler<TaskCommentAddCommand, Response>
    {
        private readonly IResponse _response;
        private readonly ITaskRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskManagerMappers _taskManagerMappers;
        private readonly ICommentRepository _commentRepository;


        public TaskCommentAddCommandHandler(IResponse response, ITaskRepository repository, IUnitOfWork unitOfWork, ITaskManagerMappers taskManagerMappers, ICommentRepository commentRepository)
        {
            _response = response;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _taskManagerMappers = taskManagerMappers;
            _commentRepository = commentRepository;
        }

        public async Task<Response> Handle(TaskCommentAddCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Comment))
                    return await _response.CreateErrorResponseAsync("Comment not be empty", HttpStatusCode.BadRequest);

                var task = await _repository.FindByIdAsync(request.TaskId);

                if (task is null)
                    return await _response.CreateErrorResponseAsync("Project not exist", HttpStatusCode.BadRequest);

                var entity = _taskManagerMappers.Map(request);

                await _commentRepository.AddAsync(entity);

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
