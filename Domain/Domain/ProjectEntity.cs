
namespace Domain.Domain
{
    public class ProjectEntity : Entity
    {
        public string? Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<TaskEntity> Tasks { get; set; } = new List<TaskEntity>();

    }
}
