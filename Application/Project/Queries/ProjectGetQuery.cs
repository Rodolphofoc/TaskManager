using Domain;
using MediatR;

namespace Applications.Project.Queries
{
    public class ProjectGetQuery : IRequest<Response>
    {
        public  Guid Id { get; set; }
    }
}
