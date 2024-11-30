using Applications.Mappers.Interface;
using Applications.Project.Commands;
using Applications.Project.Model;
using Applications.TaskManager.Commands;
using Applications.TaskManager.Model;
using AutoMapper;
using Domain.Domain;

namespace Applications.Mappers
{
    public class TaskManagerMappers : ITaskManagerMappers
    {
        private readonly IMapper _mapper;

        public TaskManagerMappers()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProjectAddCommand, ProjectEntity>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(src => src.User))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();

                cfg.CreateMap<ProjectUpdateCommand, ProjectEntity>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(x => x.Deleted, opt => opt.MapFrom(src => src.Deleted))
                .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(src => src.User))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();

                cfg.CreateMap<ProjectModel, ProjectEntity>()
                 .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Name))
                 .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(x => x.Deleted, opt => opt.MapFrom(src => src.Deleted))
                 .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();


                cfg.CreateMap<TaskAddCommand, TaskEntity>()
                    .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Title))
                    .ForMember(x => x.Priority, opt => opt.MapFrom(src => src.Priority))
                    .ForMember(x => x.Status, opt => opt.MapFrom(src => src.Status))
                    .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(src => src.User))
                    .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();

                cfg.CreateMap<TaskUpdateCommand, TaskEntity>()
                    .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Title))
                    .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(src => src.User))
                    .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();


                cfg.CreateMap<TaskModel, TaskEntity>()
                 .ForMember(x => x.Title, opt => opt.MapFrom(src => src.Name))
                 .ForMember(x => x.Deleted, opt => opt.MapFrom(src => src.Deleted))
                 .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
                 .ForMember(x => x.Priority, opt => opt.MapFrom(src => src.Priority))
                 .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Description)).ReverseMap();

                cfg.CreateMap<TaskCommentAddCommand, CommentsEntity>()
                  .ForMember(x => x.Comment, opt => opt.MapFrom(src => src.Comment))
                  .ForMember(x => x.LastModifiedBy, opt => opt.MapFrom(src => src.User))
                  .ForMember(x => x.User, opt => opt.MapFrom(src => src.User)).ReverseMap();

                
            });

            _mapper = config.CreateMapper();
        }


        public ProjectEntity Map(ProjectAddCommand command)
        {
            return _mapper.Map<ProjectAddCommand, ProjectEntity>(command);
        }

        public ProjectEntity Map(ProjectUpdateCommand command)
        {
            return _mapper.Map<ProjectUpdateCommand, ProjectEntity>(command);
        }

        public TaskEntity Map(TaskAddCommand command)
        {
            return _mapper.Map<TaskAddCommand, TaskEntity>(command);
        }

        public TaskEntity Map(TaskUpdateCommand command)
        {
            return _mapper.Map<TaskUpdateCommand, TaskEntity>(command);
        }

        public List<ProjectModel> Map(List<ProjectEntity> command)
        {
            return _mapper.Map<List<ProjectEntity>, List<ProjectModel>>(command);
        }

        public List<TaskModel> Map(List<TaskEntity> command)
        {
            return _mapper.Map<List<TaskEntity>, List<TaskModel>>(command);
        }

        public CommentsEntity Map(TaskCommentAddCommand command)
        {
            return _mapper.Map<TaskCommentAddCommand, CommentsEntity>(command);
        }
    }
}
