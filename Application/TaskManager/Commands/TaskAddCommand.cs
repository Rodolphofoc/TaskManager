using Applications.Interceptions.Model;
using Domain;
using Domain.Domain;
using MediatR;

namespace Applications.TaskManager.Commands
{
    public class TaskAddCommand : IRequest<Response>, IAuditLoggable
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Priority Priority { get; set; }
    
        public Status Status { get; set; }

        public Guid ProjectId { get; set; }

        #region Audit
        public string User { get; set; }
        public string TableName => "Task";
        public string ActionType => "Insert new Task";
        public string EntityId => string.Empty;
        public string GetChanges() => $"Title: {Title}, Description: {Description}, ProjectId: {ProjectId}, Status {Status}, User {User}";
        #endregion

    }
}
