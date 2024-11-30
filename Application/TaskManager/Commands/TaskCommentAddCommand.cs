using Applications.Interceptions.Model;
using Domain;
using MediatR;

namespace Applications.TaskManager.Commands
{
    public class TaskCommentAddCommand : IRequest<Response>, IAuditLoggable
    {
        public Guid TaskId { get; set; }
        public string Comment { get; set; }

        public string User { get; set; }

        #region Audit
        public string TableName => "Comment";
        public string ActionType => "Insert";
        public string EntityId => string.Empty;
        public string GetChanges() => $"Comment: {Comment}, Task: {TaskId.ToString()}, User: {User}";
        #endregion
    }
}
