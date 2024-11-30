using System.Net;
using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Applications.Project.Commands;
using Domain;
using MediatR;

namespace Applications.TaskManager.Commands.Handlers
{
    public class TaskUpdateCommandHandler : IRequestHandler<TaskUpdateCommand, Response>
    {
        private readonly IResponse _response;
        private readonly ITaskRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskManagerMappers _taskManagerMappers;


        public TaskUpdateCommandHandler(IResponse response, ITaskRepository repository, ITaskManagerMappers taskManagerMappers, IUnitOfWork unitOfWork)
        {
            _response = response;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _taskManagerMappers = taskManagerMappers;
        }

        public async Task<Response> Handle(TaskUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.FindByIdAsNoTrackingAsync(request.GetId());

                if (entity == null)
                    return await _response.CreateErrorResponseAsync(null, HttpStatusCode.NotFound);


                entity.Description = request.Description;
                entity.Title = request.Title;
                entity.Deleted = request.Deleted;
                entity.LastModifiedBy = request.User;

                await _repository.UpdateAsync(entity);

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
