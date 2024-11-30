using System.Net;
using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Applications.TaskManager.Commands;
using Applications.TaskManager.Commands.Handlers;
using Domain;
using Domain.Domain;
using Moq;

namespace Application.Test.HandlersTests.TaskManger
{
    public class TaskCommentAddCommandHandlerTests
    {
        private readonly Mock<IResponse> _responseMock;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITaskManagerMappers> _taskManagerMappersMock;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly TaskCommentAddCommandHandler _handler;

        public TaskCommentAddCommandHandlerTests()
        {
            _responseMock = new Mock<IResponse>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _taskManagerMappersMock = new Mock<ITaskManagerMappers>();
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _handler = new TaskCommentAddCommandHandler(
                _responseMock.Object,
                _taskRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _taskManagerMappersMock.Object,
                _commentRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_GivenValidCommand_ShouldReturnSuccessResponse()
        {
            // Arrange
            var command = new TaskCommentAddCommand
            {
                TaskId = Guid.NewGuid(),
                Comment = "New comment",
                User = "User1"
            };

            var taskEntity = new TaskEntity
            {
                Id = command.TaskId,
                Title = "Test Task",
                Description = "Test Description",
                DeadLine = DateTime.Now.AddDays(2),
                Status = Status.Pending,
                Priority = Priority.High
            };

            var commentEntity = new CommentsEntity
            {
                Comment = command.Comment,
                User = command.User,
                TaskId = command.TaskId
            };

            _taskRepositoryMock.Setup(r => r.FindByIdAsync(command.TaskId)).ReturnsAsync(taskEntity);
            _taskManagerMappersMock.Setup(m => m.Map(command)).Returns(commentEntity);
            _commentRepositoryMock.Setup(c => c.AddAsync(commentEntity)).ReturnsAsync(commentEntity);

            _responseMock.Setup(r => r.CreateSuccessResponseAsync(null, string.Empty))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            _commentRepositoryMock.Verify(c => c.AddAsync(commentEntity), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_GivenInvalidTaskId_ShouldReturnErrorResponse()
        {
            // Arrange
            var command = new TaskCommentAddCommand
            {
                TaskId = Guid.NewGuid(),
                Comment = "New comment",
                User = "User1"
            };

            _taskRepositoryMock.Setup(r => r.FindByIdAsync(command.TaskId)).ReturnsAsync((TaskEntity)null);

            _responseMock.Setup(r => r.CreateErrorResponseAsync("Project not exist", HttpStatusCode.BadRequest))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.BadRequest });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            _commentRepositoryMock.Verify(c => c.AddAsync(It.IsAny<CommentsEntity>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
