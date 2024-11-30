using Applications.Interfaces.Repository;
using Domain.Domain;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class AuditLogRepository : Repository<AuditLogEntity>, IAuditLogRepository
    {
        private readonly TaskManagerContext _context;

        public AuditLogRepository(TaskManagerContext context) : base(context)
        {
            _context = context;
        }
    }
}
