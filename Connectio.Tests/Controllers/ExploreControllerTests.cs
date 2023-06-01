using Connectio.Controllers;
using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connectio.Tests.Controllers
{
    public class ExploreControllerTests
    {
        private ApplicationUser MockUser => Mock.Of<ApplicationUser>();
        private List<Post> GetPosts() 
        {
            return new List<Post>()
            {
                new Post() { Id = 1, Text = "Example1", Created = new DateTime(2023, 04, 09, 10, 0, 0), User = MockUser },
                new Post() { Id = 2, Text = "Example2", Created = new DateTime(2023, 04, 09, 9, 0, 0), User = MockUser },
                new Post() { Id = 3, Text = "Example3", Created = new DateTime(2023, 04, 09, 11, 0, 0), User = MockUser}
            };
        }
        
        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfPosts()
        {
            // Arrange
            var mockRepo = new Mock<IPostRepository>();
            mockRepo.Setup(repo => repo.GetAllPosts()).Returns(GetPosts());
            var controller = new ExploreController(mockRepo.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ReadPostViewModel>>(viewResult.ViewData.Model);
            model.Should().HaveCount(3);
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAnEmptyListOfPosts()
        {
            // Arrange
            var mockRepo = new Mock<IPostRepository>();
            mockRepo.Setup(repo => repo.GetAllPosts()).Returns(new List<Post>());
            var controller = new ExploreController(mockRepo.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ReadPostViewModel>>(viewResult.ViewData.Model);
            model.Should().BeEmpty();
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAnOrderedListOfPosts()
        {
            // Arrange
            var mockRepo = new Mock<IPostRepository>();
            mockRepo.Setup(repo => repo.GetAllPosts()).Returns(GetPosts());
            var controller = new ExploreController(mockRepo.Object);
            var expectedResult = GetPosts().OrderByDescending(p => p.Created).Select(p => new ReadPostViewModel(p));

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<ReadPostViewModel>>(viewResult.ViewData.Model);
            model.Should().BeEquivalentTo(expectedResult);
        }
    }
}
