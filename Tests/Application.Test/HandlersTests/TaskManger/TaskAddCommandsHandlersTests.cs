using System.Net;
using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Applications.TaskManager.Commands;
using Applications.TaskManager.Commands.Handlers;
using Domain;
using Domain.Domain;
using Moq;

namespace Application.Test.HandlersTests.TaskManager
{
    public class TaskAddCommandHandlerTests
    {
        private readonly Mock<IResponse> _mockResponse;
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProjectRepository> _mockProjectRepository;
        private readonly Mock<ITaskManagerMappers> _mockTaskManagerMappers;
        private readonly TaskAddCommandHandler _handler;

        public TaskAddCommandHandlerTests()
        {
            _mockResponse = new Mock<IResponse>();
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProjectRepository = new Mock<IProjectRepository>();
            _mockTaskManagerMappers = new Mock<ITaskManagerMappers>();

            _handler = new TaskAddCommandHandler(
                _mockResponse.Object,
                _mockTaskRepository.Object,
                _mockUnitOfWork.Object,
                _mockTaskManagerMappers.Object,
                _mockProjectRepository.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenTitleOrDescriptionIsEmpty()
        {
            // Arrange
            var command = new TaskAddCommand
            {
                Title = string.Empty,
                Description = string.Empty,
                ProjectId = Guid.NewGuid()
            };

            var expectedResponse = new Response
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            _mockResponse
                .Setup(r => r.CreateErrorResponseAsync("Name or description be empty", HttpStatusCode.BadRequest))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            _mockResponse.Verify(r => r.CreateErrorResponseAsync("Name or description be empty", HttpStatusCode.BadRequest), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenProjectDoesNotExist()
        {
            // Arrange
            var command = new TaskAddCommand
            {
                Title = "New Task",
                Description = "Task Description",
                ProjectId = Guid.NewGuid()
            };

            _mockProjectRepository
                .Setup(r => r.GetByIdAsync(command.ProjectId))
                .ReturnsAsync((ProjectEntity?)null);

            var expectedResponse = new Response
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            _mockResponse
                .Setup(r => r.CreateErrorResponseAsync("Project not exist", HttpStatusCode.BadRequest))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockResponse.Verify(r => r.CreateErrorResponseAsync("Project not exist", HttpStatusCode.BadRequest), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnBadRequest_WhenTaskLimitReached()
        {
            // Arrange
            var command = new TaskAddCommand
            {
                Title = "New Task",
                Description = "Task Description",
                ProjectId = Guid.NewGuid()
            };

            var project = new ProjectEntity
            {
                Id = command.ProjectId,
                Tasks = new List<TaskEntity>(new TaskEntity[20])
            };

            _mockProjectRepository
                .Setup(r => r.GetByIdAsync(command.ProjectId))
                .ReturnsAsync(project);

            var expectedResponse = new Response
            {
                StatusCode = HttpStatusCode.BadRequest
            };

            _mockResponse
                .Setup(r => r.CreateErrorResponseAsync("Task limit reached", HttpStatusCode.BadRequest))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockResponse.Verify(r => r.CreateErrorResponseAsync("Task limit reached", HttpStatusCode.BadRequest), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldAddTaskSuccessfully()
        {
            // Arrange
            var command = new TaskAddCommand
            {
                Title = "New Task",
                Description = "Task Description",
                ProjectId = Guid.NewGuid(),
                User = "TestUser"
            };

            var project = new ProjectEntity
            {
                Id = command.ProjectId,
                Tasks = new List<TaskEntity>()
            };

            var taskEntity = new TaskEntity
            {
                Title = command.Title,
                Description = command.Description,
                ProjectId = command.ProjectId,
                LastModifiedBy = command.User
            };

            _mockProjectRepository
                .Setup(r => r.GetByIdAsync(command.ProjectId))
                .ReturnsAsync(project);

            _mockTaskManagerMappers
                .Setup(m => m.Map(command))
                .Returns(taskEntity);

            var expectedResponse = new Response
            {
                StatusCode = HttpStatusCode.OK
            };

            _mockResponse
                .Setup(r => r.CreateSuccessResponseAsync(null, string.Empty))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockTaskManagerMappers.Verify(m => m.Map(command), Times.Once);
            _mockTaskRepository.Verify(r => r.AddAsync(It.Is<TaskEntity>(t =>
                t.Title == command.Title &&
                t.Description == command.Description &&
                t.ProjectId == command.ProjectId &&
                t.LastModifiedBy == command.User
            )), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockResponse.Verify(r => r.CreateSuccessResponseAsync(null, string.Empty), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var command = new TaskAddCommand
            {
                Title = "New Task",
                Description = "Task Description",
                ProjectId = Guid.NewGuid()
            };

            _mockProjectRepository
                .Setup(r => r.GetByIdAsync(command.ProjectId))
                .ThrowsAsync(new Exception());

            var expectedResponse = new Response
            {
                StatusCode = HttpStatusCode.InternalServerError
            };

            _mockResponse
                .Setup(r => r.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            _mockProjectRepository.Verify(r => r.GetByIdAsync(command.ProjectId), Times.Once);
            _mockResponse.Verify(r => r.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError), Times.Once);
        }
    }
}
