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
    public class TaskUpdateCommandHandlerTests
    {
        private readonly Mock<IResponse> _responseMock;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ITaskManagerMappers> _taskManagerMappersMock;
        private readonly TaskUpdateCommandHandler _handler;

        public TaskUpdateCommandHandlerTests()
        {
            _responseMock = new Mock<IResponse>();
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _taskManagerMappersMock = new Mock<ITaskManagerMappers>();

            _handler = new TaskUpdateCommandHandler(
                _responseMock.Object,
                _taskRepositoryMock.Object,
                _taskManagerMappersMock.Object,
                _unitOfWorkMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenTaskDoesNotExist()
        {
            // Arrange
            var command = new TaskUpdateCommand {User = "TestUser", Description = "New Description", Title = "New Title" };
            command.SetId( Guid.NewGuid() );


            _taskRepositoryMock.Setup(r => r.FindByIdAsNoTrackingAsync(command.GetId()))
                               .ReturnsAsync((TaskEntity)null);
            _responseMock.Setup(r => r.CreateErrorResponseAsync(null, HttpStatusCode.NotFound))
                         .ReturnsAsync(new Response { StatusCode = HttpStatusCode.NotFound });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
            _taskRepositoryMock.Verify(r => r.FindByIdAsNoTrackingAsync(command.GetId()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldUpdateTask_WhenTaskExists()
        {
            // Arrange
            var command = new TaskUpdateCommand { User = "TestUser", Description = "New Description", Title = "New Title", Deleted = false };
            command.SetId(Guid.NewGuid());

            var taskEntity = new TaskEntity { Id = command.GetId(), Title = "Old Title", Description = "Old Description", Deleted = false };

            _taskRepositoryMock.Setup(r => r.FindByIdAsNoTrackingAsync(command.GetId())).ReturnsAsync(taskEntity);
            _responseMock.Setup(r => r.CreateSuccessResponseAsync(null, string.Empty))
                         .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("New Title", taskEntity.Title);
            Assert.Equal("New Description", taskEntity.Description);
            Assert.Equal("TestUser", taskEntity.LastModifiedBy);
            _taskRepositoryMock.Verify(r => r.FindByIdAsNoTrackingAsync(command.GetId()), Times.Once);
            _taskRepositoryMock.Verify(r => r.UpdateAsync(taskEntity), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var command = new TaskUpdateCommand { User = "TestUser", Description = "New Description", Title = "New Title" };
            command.SetId(Guid.NewGuid());

            _taskRepositoryMock.Setup(r => r.FindByIdAsNoTrackingAsync(command.GetId())).Throws(new Exception());
            _responseMock.Setup(r => r.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError))
                         .ReturnsAsync(new Response { StatusCode = HttpStatusCode.InternalServerError });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            _taskRepositoryMock.Verify(r => r.FindByIdAsNoTrackingAsync(command.GetId()), Times.Once);
        }
    }
}
