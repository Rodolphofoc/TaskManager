using Domain.Domain;

namespace Applications.Interfaces.Repository
{
    public interface IProjectRepository : IRepository<ProjectEntity>
    {
        Task<(List<ProjectEntity> entities, int totalPage, int totalRecords)> Filter(string name, bool? deleted, int pageSize, int pageNumber);

        Task<ProjectEntity> GetByIdAsync(Guid id);
    }
}
