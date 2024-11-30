using System.Net;
using Applications.Interfaces.Repository;
using Applications.TaskManager.Queries;
using Applications.TaskManager.Queries.Handlers;
using Domain;
using Domain.Domain;
using Moq;

namespace Application.Test.HandlersTests.TaskManger
{
    public class TaskGetQueryHandlerTests
    {
        private readonly Mock<IResponse> _mockResponse;
        private readonly Mock<ITaskRepository> _mockRepository;
        private readonly TaskGetQueryHandler _handler;

        public TaskGetQueryHandlerTests()
        {
            _mockResponse = new Mock<IResponse>();
            _mockRepository = new Mock<ITaskRepository>();
            _handler = new TaskGetQueryHandler(_mockResponse.Object, _mockRepository.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenTaskIsNotFound()
        {
            // Arrange
            var query = new TaskGetQuery { Id = Guid.NewGuid() }; // Id fictício
            _mockRepository.Setup(repo => repo.FindById(It.IsAny<Guid>())).Returns((TaskEntity)null); // Simula tarefa não encontrada

            _mockResponse.Setup(res => res.CreateErrorResponseAsync(null, HttpStatusCode.NotFound))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.NotFound });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturnNotFound_WhenTaskIsDeleted()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var query = new TaskGetQuery { Id = taskId };
            var taskEntity = new TaskEntity { Id = taskId, Deleted = true };
            _mockRepository.Setup(repo => repo.FindById(taskId)).Returns(taskEntity); // Simula tarefa deletada

            _mockResponse.Setup(res => res.CreateErrorResponseAsync(null, HttpStatusCode.NotFound))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.NotFound });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenTaskIsFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var query = new TaskGetQuery { Id = taskId };
            var taskEntity = new TaskEntity { Id = taskId, Deleted = false }; // Tarefa não deletada
            _mockRepository.Setup(repo => repo.FindById(taskId)).Returns(taskEntity); // Simula tarefa encontrada

            _mockResponse.Setup(res => res.CreateSuccessResponseAsync(It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK, Data = taskEntity });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(taskEntity, result.Data);
        }

        [Fact]
        public async Task Handle_ShouldReturnInternalServerError_WhenExceptionOccurs()
        {
            // Arrange
            var query = new TaskGetQuery { Id = Guid.NewGuid() };
            _mockRepository.Setup(repo => repo.FindById(It.IsAny<Guid>())).Throws(new Exception("Unexpected error"));

            _mockResponse.Setup(res => res.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.InternalServerError });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        }
    }
}
