using Domain.Domain;

namespace Applications.Interfaces.Repository
{
    public interface  ITaskRepository : IRepository<TaskEntity>
    {
        Task<(List<TaskEntity> entities, int totalPage, int totalRecords)> Filter(string name, Priority? priority, bool? deleted, int pageSize, int pageNumber, Guid projectId);

        Task<List<TaskEntity>> GetAllTaskClosed();
    }
}
