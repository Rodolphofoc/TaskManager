using Applications.Interfaces.Repository;
using Domain.Domain;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{

    public class TaskRepository : Repository<TaskEntity>, ITaskRepository
    {
        private readonly TaskManagerContext _context;

        public TaskRepository(TaskManagerContext context) : base(context)
        {
            _context = context;
        }

        public async Task<(List<TaskEntity> entities, int totalPage, int totalRecords)> Filter(string name, Priority? priority,  bool? deleted, int pageSize, int pageNumber, Guid projectId)
        {
            int skip = (pageNumber - 1) * pageSize;

            var query = _context.Task.Where(x => x.ProjectId == projectId).AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Title.ToLower() == name.ToLower()).AsQueryable();

            if (deleted is not null)
                query = query.Where(x => x.Deleted == deleted).AsQueryable();

            if (priority is not null)
                query = query.Where(x => x.Priority == priority).AsQueryable();

            var totalPage = (int)Math.Ceiling((double)_context.Project.Count() / pageSize);

            var totalRecords = query.Count();

            return (entities: await query.Skip(skip).Take(pageSize).AsNoTracking().ToListAsync(), totalPage: totalPage, totalRecords: totalRecords);
        }


        public async Task<List<TaskEntity>> GetAllTaskClosed()
        {
            return await _context.Task.Where(x => x.Status == Status.Done && x.LastModified > DateTime.Now.AddDays(-30)).ToListAsync();
        }

    }

}
