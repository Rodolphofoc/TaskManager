using Domain;
using Domain.Domain;
using MediatR;

namespace Applications.TaskManager.Queries
{
    public class TaskListQuery : IRequest<Response>
    {
        public string? Name { get; set; }
        public int? PageSize { get; set; }

        public int? Page { get; set; }

        public bool? Deleted { get; set; }

        public Priority? Priority { get; set; }

        public string User { get; set; }

        public Guid ProjectId { get; set; }

        public TaskListQuery()
        {
            PageSize = 10;
            Page = 1;
        }
    }
}
