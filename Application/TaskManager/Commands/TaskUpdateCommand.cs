using Applications.Interceptions.Model;
using Domain;
using MediatR;

namespace Applications.TaskManager.Commands
{
    public class TaskUpdateCommand : IRequest<Response>, IAuditLoggable
    {
        private Guid Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool Deleted { get; set; }

        public string User { get; set; }

        public Guid ProjectId { get; set; }


        public void SetId(Guid id) { Id = id; }
        public Guid GetId()
        {
            return Id;
        }


        #region audit
        public string TableName => "Task";
        public string ActionType => "Update Task";
        public string EntityId => Id.ToString();

        public string GetChanges() => $"User: {User}, Title : {Title} Description:{Description}, ProjectId: {ProjectId}";

        #endregion
    }
}
