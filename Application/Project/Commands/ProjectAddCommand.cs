using System;
using Applications.Interceptions.Model;
using Domain;
using MediatR;

namespace Applications.Project.Commands
{
    public class ProjectAddCommand : IRequest<Response>, IAuditLoggable
    {

        public string? Name { get; set; }

        public string Description { get; set; }

        #region Audit
        public string User { get; set; }
        public string TableName => "Project";
        public string ActionType => "Insert";
        public string EntityId => string.Empty;
        public string GetChanges() => $"Title: {Name}, Description: {Description}, User {User}";
        #endregion

    }
}
