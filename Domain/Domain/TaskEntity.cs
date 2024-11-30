namespace Domain.Domain
{
    public class TaskEntity : Entity
    {

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime DeadLine { get; set; }

        public Guid ProjectId { get; set; }
        public virtual ProjectEntity Project { get; set; }

        public Priority Priority { get; set; }

        public Status Status { get; set; }

        public virtual ICollection<CommentsEntity> Comments { get; set; } = new List<CommentsEntity>();

    }
}
