using System.Net;
using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Domain;
using MediatR;

namespace Applications.Project.Commands.Handlers
{
    public class ProjectUpdateCommandHandler : IRequestHandler<ProjectUpdateCommand, Response>
    {
        private readonly IResponse _response;
        private readonly IProjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITaskManagerMappers _taskManagerMappers;


        public ProjectUpdateCommandHandler(IResponse response, IProjectRepository repository, ITaskManagerMappers taskManagerMappers, IUnitOfWork unitOfWork)
        {
            _response = response;
            _repository = repository;
            _unitOfWork = unitOfWork;
            _taskManagerMappers = taskManagerMappers;
        }

        public async Task<Response> Handle(ProjectUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity = await _repository.FindByIdAsNoTrackingAsync(request.GetId());

                if (entity == null)
                    return await _response.CreateErrorResponseAsync(null, HttpStatusCode.NotFound);


                entity.Description = request.Description;
                entity.Name = request.Name;
                entity.Deleted = request.Deleted;

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
