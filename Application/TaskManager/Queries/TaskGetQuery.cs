using Domain;
using MediatR;

namespace Applications.TaskManager.Queries
{
    public class TaskGetQuery : IRequest<Response>
    {
        public Guid Id { get; set; }

    }
}
