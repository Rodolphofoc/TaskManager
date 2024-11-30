using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Applications.Interfaces.Repository;
using Applications.Mappers;
using Applications.Mappers.Interface;
using Infrastructure.Context;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCuting
{
    [ExcludeFromCodeCoverage]
     public static class ServicesDependencyInjection
    {
        private const string applicationProjectName = "Applications";


        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.Load(applicationProjectName);

            services.AddDbContext<TaskManagerContext>(options =>
                 options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                 o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITaskManagerMappers, TaskManagerMappers>();

            services.AddAutoMapper(assembly);

            services.AddMediatr();
            return services;

        }

        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<IAuditLogRepository, AuditLogRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();


            return services;
        }

        private static void AddMediatr(this IServiceCollection services)
        {
            var assembly = Assembly.Load(applicationProjectName);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(Assembly.Load(applicationProjectName)));
        }
    }
}
