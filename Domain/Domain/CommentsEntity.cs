namespace Domain.Domain
{
    public class CommentsEntity : Entity
    {
        public string Comment { get; set; }

        public string User { get; set; }

        public Guid TaskId { get; set; }

        public virtual TaskEntity Task { get; set; }
    }
}
