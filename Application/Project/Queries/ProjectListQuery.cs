using Domain;
using MediatR;

namespace Applications.Project.Queries
{
    public class ProjectListQuery : IRequest<Response>
    {
        public string? Name { get; set; }
        public int? PageSize { get; set; }

        public int? Page { get; set; }

        public bool? Deleted { get; set; }

        public string User { get; set; }

        public ProjectListQuery()
        {
            PageSize = 10;
            Page = 1;
        }

    }
}
