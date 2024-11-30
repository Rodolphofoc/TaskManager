using Domain;
using MediatR;

namespace Applications.TaskManager.Queries
{
    public class TaskMediaReportQuery : IRequest<Response>
    {
        public string Profile { get; set; }
    }
}
