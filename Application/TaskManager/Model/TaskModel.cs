using Domain.Domain;

namespace Applications.TaskManager.Model
{
    public class TaskModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public string Description { get; set; }

        public bool? Deleted { get; set; }

        public Priority Priority { get; set; }
    }
}
