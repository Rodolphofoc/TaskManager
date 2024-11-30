using Applications.Interfaces.Repository;
using Domain.Domain;
using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProjectRepository : Repository<ProjectEntity>, IProjectRepository
    {
        private readonly TaskManagerContext _context;

        public ProjectRepository(TaskManagerContext context) : base(context)
        { 
            _context = context;
        }


        public async Task<(List<ProjectEntity> entities, int totalPage, int totalRecords)> Filter(string name, bool? deleted , int pageSize, int pageNumber)
        {
            int skip = (pageNumber -1) * pageSize;

            var query = _context.Project.AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(x => x.Name.ToLower() == name.ToLower()).AsQueryable();

            if (deleted is not null)
                query = query.Where(x => x.Deleted == deleted).AsQueryable();


            var totalPage = (int)Math.Ceiling((double)_context.Project.Count() /pageSize);

            var totalRecords = query.Count();

            return (entities: await query.Skip(skip).Take(pageSize).AsNoTracking().ToListAsync(), totalPage: totalPage, totalRecords: totalRecords);
        }


        public async Task<ProjectEntity> GetByIdAsync(Guid id)
        {
            return await _context.Project.Where(x => x.Id == id)
                .Include(x => x.Tasks).FirstOrDefaultAsync();
        }
    }
}
