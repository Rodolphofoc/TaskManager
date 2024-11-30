using System.Net;
using System.Xml.Linq;
using Applications.Abstracts;
using Applications.Interfaces.Repository;
using Applications.Mappers.Interface;
using Applications.Project.Model;
using Applications.Project.Queries;
using Applications.Project.Queries.Handlers;
using Domain;
using Domain.Domain;
using Moq;

namespace Application.Test.HandlersTests.Project
{
    public class ProjectListQueryHandlerTests
    {
        private readonly Mock<IResponse> _mockResponse;
        private readonly Mock<IProjectRepository> _mockRepository;
        private readonly Mock<ITaskManagerMappers> _mockTaskManagerMappers;

        public ProjectListQueryHandlerTests()
        {
            _mockResponse = new Mock<IResponse>();
            _mockRepository = new Mock<IProjectRepository>();
            _mockTaskManagerMappers = new Mock<ITaskManagerMappers>();
        }

        [Fact]
        public async Task Handle_WhenProjectsFound_ReturnsPagedListWithProjects()
        {
            // Arrange
            var handler = new ProjectListQueryHandler(
                _mockResponse.Object,
                _mockRepository.Object,
                _mockTaskManagerMappers.Object
            );

            var query = new ProjectListQuery
            {
                Name = "Project1",
                Deleted = false,
                PageSize = 10,
                Page = 1
            };

            var entities = new List<ProjectEntity>
            {
                new ProjectEntity { Id = Guid.NewGuid(), Name = "Project1", Deleted = false },
                new ProjectEntity { Id = Guid.NewGuid(), Name = "Project2", Deleted = false }
            };

    
            // Mocking repository call
            _mockRepository.Setup(r => r.Filter(query.Name, query.Deleted, query.PageSize.Value, query.Page.Value))
                .ReturnsAsync((entities : entities, totalPage: 1, totalRecords: 10));

            // Mocking the mapper call
            _mockTaskManagerMappers.Setup(m => m.Map(It.IsAny<List<ProjectEntity>>()))
                .Returns(new List<ProjectModel>()
                {
                   new ProjectModel(){Id = Guid.NewGuid(), Name = "Project1", Deleted = false },
                    new ProjectModel(){Id = Guid.NewGuid(), Name = "Project2", Deleted = false}
                });

            // Mocking the response
            _mockResponse.Setup(r => r.CreateSuccessResponseAsync(It.IsAny<object>(), string.Empty))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenNoProjectsFound_ReturnsEmptyPagedList()
        {
            // Arrange
            var handler = new ProjectListQueryHandler(
                _mockResponse.Object,
                _mockRepository.Object,
                _mockTaskManagerMappers.Object
            );

            var query = new ProjectListQuery
            {
                Name = "NonExistingProject",
                Deleted = false,
                PageSize = 10,
                Page = 1
            };

            var entities = new List<ProjectEntity>
            {
                new ProjectEntity { Id = Guid.NewGuid(), Name = "Project1", Deleted = false },
                new ProjectEntity { Id = Guid.NewGuid(), Name = "Project2", Deleted = false }
            };

            // Mocking repository call
            _mockRepository.Setup(r => r.Filter(query.Name, query.Deleted, query.PageSize.Value, query.Page.Value))
                .ReturnsAsync((entities: entities, totalPage: 1, totalRecords: 10));

            // Mocking the mapper call
            _mockTaskManagerMappers.Setup(m => m.Map(It.IsAny<List<ProjectEntity>>()))
                .Returns(new List<ProjectModel>());

            // Mocking the response
            _mockResponse.Setup(r => r.CreateSuccessResponseAsync(It.IsAny<object>(), string.Empty))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.OK });

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
        }

        [Fact]
        public async Task Handle_WhenExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var handler = new ProjectListQueryHandler(
                _mockResponse.Object,
                _mockRepository.Object,
                _mockTaskManagerMappers.Object
            );

            var query = new ProjectListQuery
            {
                Name = "Project1",
                Deleted = false,
                PageSize = 10,
                Page = 1
            };

            // Simulating an exception in the repository
            _mockRepository.Setup(r => r.Filter(query.Name, query.Deleted, query.PageSize.Value, query.Page.Value))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Mocking the response
            _mockResponse.Setup(r => r.CreateErrorResponseAsync(It.IsAny<object>(), HttpStatusCode.InternalServerError))
                .ReturnsAsync(new Response { StatusCode = HttpStatusCode.InternalServerError });

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        }
    }
}
