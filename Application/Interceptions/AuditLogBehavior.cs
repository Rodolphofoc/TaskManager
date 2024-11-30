using Applications.Interceptions.Model;
using Applications.Interfaces.Repository;
using Domain.Domain;
using MediatR;

namespace Applications.Interceptions
{
    public class AuditLogBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
     where TRequest : IRequest<TResponse>
    {
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuditLogBehavior(IAuditLogRepository auditLogRepository, IUnitOfWork unitOfWork)
        {
            _auditLogRepository = auditLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var response = await next();

            if (request is IAuditLoggable loggable)
            {
                var log = new AuditLogEntity
                {
                    ActionType = loggable.ActionType,        
                    TableName = loggable.TableName,          
                    EntityId = loggable.EntityId,            
                    Changes = loggable.GetChanges(),         
                    User = loggable.User,
                    Timestamp = DateTime.UtcNow              
                };

                await _auditLogRepository.AddAsync(log);  

                await _unitOfWork.CompleteAsync();
            }

            return response;
        }
    }

}
