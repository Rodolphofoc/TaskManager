using System.Net;
using Applications.Interfaces.Repository;
using Domain;
using MediatR;

namespace Applications.Project.Commands.Handlers
{
    public class ProjectDeleteCommandHandler :  IRequestHandler<ProjectDeleteCommand, Response>
    {

        private readonly IResponse _response;
        private readonly IProjectRepository _repository;
        private readonly IUnitOfWork _unitOfWork;


        public ProjectDeleteCommandHandler(IResponse response, IProjectRepository repository, IUnitOfWork unitOfWork)
        {
            _response = response;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Response> Handle(ProjectDeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var entity =  await _repository.GetByIdAsync(request.Id);

                if (entity == null)
                    return await _response.CreateErrorResponseAsync(null, HttpStatusCode.NotFound);

                if(entity.Tasks.Any(x => !x.Deleted))
                    return await _response.CreateErrorResponseAsync("Please close all tasks before closing the project or remove the task opening", HttpStatusCode.NotFound);


                entity.Deleted = true;

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
