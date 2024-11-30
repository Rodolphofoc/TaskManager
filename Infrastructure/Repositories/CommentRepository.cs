using Applications.Interfaces.Repository;
using Domain.Domain;
using Infrastructure.Context;

namespace Infrastructure.Repositories
{
    public class CommentRepository :  Repository<CommentsEntity> , ICommentRepository
    {
        private readonly TaskManagerContext _context;

            public CommentRepository(TaskManagerContext context) : base(context)
            {
                _context = context;
            }
    }
}


