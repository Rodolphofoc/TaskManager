using System.Xml.Linq;
using Applications.Interceptions.Model;
using Domain;
using MediatR;

namespace Applications.Project.Commands
{
    public class ProjectDeleteCommand : IRequest<Response>, IAuditLoggable
    {
        public Guid Id { get; set; }
        #region Audit
        public string User { get; set; }
        public string TableName => "Project";
        public string ActionType => "Delete";
        public string EntityId => Id.ToString();
        public string GetChanges() => $" Description: Delete Project, User {User}";
        #endregion

    }
}
