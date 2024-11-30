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
    public class TaskDeleteCommandHandlerTests
    {
        private readonly Mock<IResponse> _mockResponse;
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ITaskManagerMappers> _mockTaskManagerMappers;
        private readonly TaskDeleteCommandHandler _handler;

        public TaskDeleteCommandHandlerTests()
        {
            _mockResponse = new Mock<IResponse>();
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockTaskManagerMappers = new Mock<ITaskManagerMappers>();

            _handler = new TaskDeleteCommandHandler(
                _mockResponse.Object,
                _mockTaskRepository.Object,
                _mockUnitOfWork.Object,
                _mockTaskManagerMappers.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var command = new TaskDeleteCommand
            {
                Id = Guid.NewGuid(),
                User = "TestUser"
            };

            _mockTaskRepository
                .Setup(r => r.FindById(command.Id))
                .Returns((TaskEntity?)null);

            var expectedResponse = new Response
            {
                StatusCode = HttpStatusCode.NotFound
            };

            _mockResponse
                .Setup(r => r.CreateErrorResponseAsync(null, HttpStatusCode.NotFound))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            _mockTaskRepository.Verify(r => r.FindById(command.Id), Times.Once);
            _mockResponse.Verify(r => r.CreateErrorResponseAsync(null, HttpStatusCode.NotFound), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldMarkTaskAsDeletedAndSaveChanges()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var command = new TaskDeleteCommand
            {
                Id = taskId,
                User = "TestUser"
            };

            var taskEntity = new TaskEntity
            {
                Id = taskId,
                Deleted = false,
                LastModifiedBy = null
            };

            _mockTaskRepository
                .Setup(r => r.FindById(taskId))
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
            Assert.True(taskEntity.Deleted);
            Assert.Equal(command.User, taskEntity.LastModifiedBy);
            _mockTaskRepository.Verify(r => r.FindById(taskId), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockResponse.Verify(r => r.CreateSuccessResponseAsync(null, string.Empty), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnInternalServerError_OnException()
        {
            // Arrange
            var command = new TaskDeleteCommand
            {
                Id = Guid.NewGuid(),
                User = "TestUser"
            };

            _mockTaskRepository
                .Setup(r => r.FindById(command.Id))
                .Throws(new Exception());

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
            _mockTaskRepository.Verify(r => r.FindById(command.Id), Times.Once);
            _mockResponse.Verify(r => r.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError), Times.Once);
        }
    }
}
