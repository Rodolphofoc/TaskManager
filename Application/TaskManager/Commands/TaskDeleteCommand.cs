using System.Xml.Linq;
using Applications.Interceptions.Model;
using Domain;
using MediatR;

namespace Applications.TaskManager.Commands
{
    public class TaskDeleteCommand : IRequest<Response>, IAuditLoggable
    {
        public Guid Id { get; set; }

        public string User { get; set; }

        public Guid ProjectId { get; set; }

        #region audit
        public string TableName => "Task";
        public string ActionType => "Delete";
        public string EntityId => Id.ToString(); // Deixe vazio para criação

        public string GetChanges() => $"User: {User}, Description: Remove Task, ProjectId: {ProjectId}";

        #endregion

    }
}
