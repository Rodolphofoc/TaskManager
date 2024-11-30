using System.Net;
using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Domain;
using MediatR;

namespace Applications.TaskManager.Commands.Handlers
{
    public class TaskDeleteCommandHandler : IRequestHandler<TaskDeleteCommand, Response>
    {
        private readonly IResponse _response;
        private readonly ITaskRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskManagerMappers _taskManagerMappers;


        public TaskDeleteCommandHandler(IResponse response, ITaskRepository repository, IUnitOfWork unitOfWork, ITaskManagerMappers taskManagerMappers)
        {
            _response = response;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _taskManagerMappers = taskManagerMappers;
        }

        public async Task<Response> Handle(TaskDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = _repository.FindById(request.Id);

                if (entity == null)
                    return await _response.CreateErrorResponseAsync(null, HttpStatusCode.NotFound);

                entity.Deleted = true;
                entity.LastModifiedBy = request.User;
                await _unitOfWork.CompleteAsync();

                return await _response.CreateSuccessResponseAsync(null, string.Empty);

            }
            catch (Exception)
            {
                return await _response.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError);
            }
        }
    }
}
