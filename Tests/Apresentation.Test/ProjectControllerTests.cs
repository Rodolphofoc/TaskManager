using System.Net;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using Api.Controllers;
using MediatR;
using Applications.Project.Commands;
using Applications.Project.Queries;
using Applications.Project.Model;
using Domain;

namespace Api.Tests.Controllers
{
    public class ProjectControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly ProjectController _controller;

        public ProjectControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new ProjectController(_mockMediator.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var command = new ProjectAddCommand { Name = "Test Project", Description = "Description" };
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<ProjectAddCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await _controller.Post(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task Put_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var command = new ProjectUpdateCommand { Name = "Updated Project" };
            var projectId = Guid.NewGuid();
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<ProjectUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await _controller.Put(command, projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }


        [Fact]
        public async Task Get_ShouldReturnOk_WhenProjectIsFound()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var query = new ProjectGetQuery { Id = projectId };
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<ProjectGetQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK, Data = new ProjectModel() });

            // Act
            var result = await _controller.Get(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var command = new ProjectDeleteCommand { Id = projectId };
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<ProjectDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await _controller.Delete(projectId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }


        [Fact]
        public async Task GetAll_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var query = new ProjectListQuery { Page = 1, PageSize = 10 };
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<ProjectListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK, Data = new List<ProjectModel>() });

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }
    }
}
