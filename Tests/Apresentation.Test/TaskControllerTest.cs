using System.Net;
using Api.Controllers;
using Applications.TaskManager.Commands;
using Applications.TaskManager.Model;
using Applications.TaskManager.Queries;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers
{
    public class TaskControllerTests
    {
        private readonly Mock<IMediator> _mockMediator;
        private readonly TaskController _controller;

        public TaskControllerTests()
        {
            _mockMediator = new Mock<IMediator>();
            _controller = new TaskController(_mockMediator.Object);
        }

        [Fact]
        public async Task Post_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var command = new TaskAddCommand { Title = "Test Task", Description = "Task Description" };
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<TaskAddCommand>(), It.IsAny<CancellationToken>()))
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
            var command = new TaskUpdateCommand { Title = "Updated Task" };
            var taskId = Guid.NewGuid();
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<TaskUpdateCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await _controller.Put(command, taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }


        [Fact]
        public async Task Get_ShouldReturnOk_WhenTaskIsFound()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var query = new TaskGetQuery { Id = taskId };
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<TaskGetQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK, Data = new TaskModel() });

            // Act
            var result = await _controller.Get(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }


        [Fact]
        public async Task Delete_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var command = new TaskDeleteCommand { Id = taskId };
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<TaskDeleteCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await _controller.Delete(taskId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetAll_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var query = new TaskListQuery { Page = 1, PageSize = 10 };
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<TaskListQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK, Data = new List<TaskModel>() });

            // Act
            var result = await _controller.GetAll(query);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

        [Fact]
        public async Task PostComment_ShouldReturnOk_WhenRequestIsValid()
        {
            // Arrange
            var command = new TaskCommentAddCommand { TaskId = Guid.NewGuid(), Comment = "Test Comment" };
            _mockMediator.Setup(mediator => mediator.Send(It.IsAny<TaskCommentAddCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await _controller.PostComment(command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((int)HttpStatusCode.OK, okResult.StatusCode);
        }

    }
}
