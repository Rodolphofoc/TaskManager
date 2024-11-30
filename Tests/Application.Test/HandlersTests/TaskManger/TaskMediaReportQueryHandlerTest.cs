using System.Net;
using Applications.Interfaces.Repository;
using Applications.TaskManager.Queries;
using Applications.TaskManager.Queries.Handlers;
using Domain;
using Domain.Domain;
using Moq;

namespace Application.Test.HandlersTests.TaskManger
{
    public class TaskMediaReportQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ShouldReturnForbiddenResponse_WhenProfileIsNotGerente()
        {
            // Arrange
            var mockResponse = new Mock<IResponse>();
            var mockRepository = new Mock<ITaskRepository>();

            var handler = new TaskMediaReportQueryHandler(mockResponse.Object, mockRepository.Object);

            // Configurando o mock de resposta para retornar "Forbidden" quando o perfil não for "gerente"
            mockResponse.Setup(r => r.CreateErrorResponseAsync(null, HttpStatusCode.Forbidden))
                        .ReturnsAsync(new Response { StatusCode = HttpStatusCode.Forbidden });

            // Act
            var result = await handler.Handle(new TaskMediaReportQuery { Profile = "funcionario" }, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.Forbidden, result.StatusCode);
            mockRepository.Verify(repo => repo.GetAllTaskClosed(), Times.Never); // Verifica se o repositório não foi chamado
            mockResponse.Verify(r => r.CreateErrorResponseAsync(null, HttpStatusCode.Forbidden), Times.Once); // Verifica se a resposta de "Forbidden" foi chamada
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResponse_WhenTasksAreFound()
        {
            // Arrange
            var mockResponse = new Mock<IResponse>();
            var mockRepository = new Mock<ITaskRepository>();

            // Simulando a lista de tarefas fechadas
            var tasks = new List<TaskEntity>
            {
                new TaskEntity { LastModifiedBy = "User1" },
                new TaskEntity { LastModifiedBy = "User1" },
                new TaskEntity { LastModifiedBy = "User2" }
            };

            // Configurando o repositório mockado para retornar as tarefas simuladas
            mockRepository.Setup(repo => repo.GetAllTaskClosed())
                .ReturnsAsync(tasks);

            var handler = new TaskMediaReportQueryHandler(mockResponse.Object, mockRepository.Object);

            // Preparando o mock para retornar uma resposta de sucesso com a média calculada
            mockResponse.Setup(r => r.CreateSuccessResponseAsync(It.IsAny<object>(), It.IsAny<string>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await handler.Handle(new TaskMediaReportQuery { Profile = "gerente" }, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            mockRepository.Verify(repo => repo.GetAllTaskClosed(), Times.Once); // Verifica se o repositório foi chamado
            mockResponse.Verify(r => r.CreateSuccessResponseAsync(It.IsAny<object>(), It.IsAny<string>()), Times.Once); // Verifica se o método de sucesso foi chamado
        }

        [Fact]
        public async Task Handle_ShouldReturnErrorResponse_WhenExceptionIsThrown()
        {
            // Arrange
            var mockResponse = new Mock<IResponse>();
            var mockRepository = new Mock<ITaskRepository>();

            // Configurando o repositório para lançar uma exceção
            mockRepository.Setup(repo => repo.GetAllTaskClosed())
                .ThrowsAsync(new Exception("Database error"));

            var handler = new TaskMediaReportQueryHandler(mockResponse.Object, mockRepository.Object);

            // Preparando o mock para retornar uma resposta de erro
            mockResponse.Setup(r => r.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.InternalServerError });

            // Act
            var result = await handler.Handle(new TaskMediaReportQuery { Profile = "gerente" }, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            mockRepository.Verify(repo => repo.GetAllTaskClosed(), Times.Once); // Verifica se o repositório foi chamado
            mockResponse.Verify(r => r.CreateErrorResponseAsync(null, HttpStatusCode.InternalServerError), Times.Once); // Verifica se o método de erro foi chamado
        }
    }
}
