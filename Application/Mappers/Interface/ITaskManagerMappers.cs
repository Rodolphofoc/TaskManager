using Applications.Project.Commands;
using Applications.Project.Model;
using Applications.TaskManager.Commands;
using Applications.TaskManager.Model;
using Domain.Domain;

namespace Applications.Mappers.Interface
{
    public interface ITaskManagerMappers
    {
        ProjectEntity Map(ProjectAddCommand command);

        ProjectEntity Map(ProjectUpdateCommand command);

        TaskEntity Map(TaskAddCommand command);

        TaskEntity Map(TaskUpdateCommand command);

        List<ProjectModel> Map(List<ProjectEntity> command);

        List<TaskModel> Map(List<TaskEntity> command);

        CommentsEntity Map(TaskCommentAddCommand command);
    }
}
