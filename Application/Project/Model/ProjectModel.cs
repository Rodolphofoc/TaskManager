namespace Applications.Project.Model
{
    public class ProjectModel
    {
        public Guid Id { get; set; }

        public string? Name { get; set; }

        public string Description { get; set; }

        public bool? Deleted { get; set; }
    }
}
