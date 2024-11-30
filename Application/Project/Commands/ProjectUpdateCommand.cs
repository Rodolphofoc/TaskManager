using Applications.Interceptions.Model;
using Domain;
using MediatR;

namespace Applications.Project.Commands
{
    public class ProjectUpdateCommand : IRequest<Response>, IAuditLoggable
    {
        private Guid Id { get; set; }
        public string? Name { get; set; }

        public string? Description { get; set; }

        public bool Deleted { get; set; }
        public void SetId(Guid id) { Id = id; }
        public Guid GetId()
        {
            return Id;
        }

        #region Audit
        public string User { get; set; }
        public string TableName => "Project";
        public string ActionType => "Update";
        public string EntityId => Id.ToString();
        public string GetChanges() => $" Name: {Name}, Description:{Description} , User: {User}, Deleted: {Deleted}";
        #endregion


    }
}
