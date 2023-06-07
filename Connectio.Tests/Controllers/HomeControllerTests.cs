using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Connectio.Tests.Controllers;

public class HomeControllerTests
{
    private readonly Mock<IReactionRepository> mockReactionRepo;
    private readonly Mock<IPostRepository> mockPostRepo;
    private readonly Mock<IUserRepository> mockUserRepo;
    private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
    private readonly ApplicationUser mockUser;
    private readonly ApplicationUser mockFollower;
    private readonly List<Post> mockPosts;
    private readonly List<Like> mockLikes;
    private readonly List<Comment> mockComments;


    public HomeControllerTests()
    {
        mockReactionRepo = new Mock<IReactionRepository>();
        mockPostRepo = new Mock<IPostRepository>();
        mockUserRepo = new Mock<IUserRepository>();
        mockUserManager = MockHelpers.MockUserManager<ApplicationUser>();
        mockUser = Mock.Of<ApplicationUser>();
        mockFollower = Mock.Of<ApplicationUser>();
        mockPosts = new()
        {
            new Post() { Id = 1, Text = "Example1", Created = new DateTime(2023, 04, 09, 10, 0, 0), User = mockUser },
            new Post() { Id = 2, Text = "Example2", Created = new DateTime(2023, 04, 09, 9, 0, 0), User = mockUser },
            new Post() { Id = 3, Text = "Example3", Created = new DateTime(2023, 04, 09, 11, 0, 0), User = mockUser},
            new Post() { Id = 4, Text = "Example4", Created = new DateTime(2023, 04, 09, 9, 30, 0), User = mockUser},
            new Post() { Id = 5, Text = "Example5", Created = new DateTime(2023, 04, 09, 11, 10, 0), User = mockUser},
            new Post() { Id = 6, Text = "Example6", Created = new DateTime(2023, 04, 09, 11, 15, 0), User = mockUser}
        };
        mockLikes = new()
        {
            new Like() { User = mockFollower, Post= mockPosts[0], Created = new DateTime(2023, 04, 09, 11, 0, 0) },
            new Like() { User = mockFollower, Post= mockPosts[1], Created = new DateTime(2023, 04, 09, 10, 0, 0) },
            new Like() { User = mockFollower, Post= mockPosts[2], Created = new DateTime(2023, 04, 09, 12, 0, 0) }
        };
        mockComments = new()
        {
            new Comment() { Id = 1, Post = mockPosts[1], User = mockFollower, Created = new DateTime(2023, 04, 10, 10, 0, 0), Text = "Comment1"},
            new Comment() { Id = 2, Post = mockPosts[2], User = mockFollower, Created = new DateTime(2023, 04, 10, 12, 0, 0), Text = "Comment2"},
            new Comment() { Id = 3, Post = mockPosts[3], User = mockFollower, Created = new DateTime(2023, 04, 10, 11, 0, 0), Text = "Comment3"}
        };

        mockFollower.UserName = "Test";
        mockFollower.DisplayName = "Test";

        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);
        mockReactionRepo.Setup(repo => repo.GetAllUserLikes(It.IsAny<ApplicationUser>())).Returns(mockLikes);
    }

    [Fact]
    public async Task Index_ReturnsANotFoundResult_WhenUserNotLoggedIn()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);
        var controller = new HomeController(mockPostRepo.Object, mockUserManager.Object, mockUserRepo.Object, mockReactionRepo.Object);

        // Act
        var result = await controller.Index();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithNoPosts()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);
        var controller = new HomeController(mockPostRepo.Object, mockUserManager.Object, mockUserRepo.Object, mockReactionRepo.Object);

        // Act
        var result = await controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeAssignableTo<IEnumerable<ReadPostViewModel>>().Which.Should().BeEmpty();
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithNewPosts()
    {
        // Arrange
        mockUser.Following.Add(mockFollower);
        mockPostRepo.Setup(repo => repo.GetAllPostsByUser(It.IsAny<string>())).Returns(mockPosts);
        mockReactionRepo.Setup(repo => repo.GetAllUserComments(It.IsAny<ApplicationUser>())).Returns(new List<Comment>());
        mockReactionRepo.Setup(repo => repo.GetAllUserLikes(It.IsAny<ApplicationUser>())).Returns(new List<Like>());

        var controller = new HomeController(mockPostRepo.Object, mockUserManager.Object, mockUserRepo.Object, mockReactionRepo.Object);

        var expectedActivityType = ActivityType.New;

        // Act
        var result = await controller.Index();

        // Assert
        var model = result.As<ViewResult>().ViewData.Model.As<IEnumerable<ReadPostViewModel>>();
        model.Should().AllSatisfy(p =>
        {
            p.Header.Should().BeNull();
            p.ActivityType.Should().Be(expectedActivityType);
        });
        model.Should().BeInDescendingOrder(p => p.ActivityCreated);
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithLikedPosts()
    {
        // Arrange
        mockUser.Following.Add(mockFollower);
        mockPostRepo.Setup(repo => repo.GetAllPostsByUser(It.IsAny<string>())).Returns(new List<Post>());
        mockReactionRepo.Setup(repo => repo.GetAllUserComments(It.IsAny<ApplicationUser>())).Returns(new List<Comment>());

        var controller = new HomeController(mockPostRepo.Object, mockUserManager.Object, mockUserRepo.Object, mockReactionRepo.Object);

        var expectedHeader = "Test (@Test) liked";
        var expectedActivityType = ActivityType.Like;

        // Act
        var result = await controller.Index();

        // Assert
        var model = result.As<ViewResult>().ViewData.Model.As<IEnumerable<ReadPostViewModel>>();
        model.Should().AllSatisfy(p =>
        {
            p.Header.Should().Be(expectedHeader);
            p.ActivityType.Should().Be(expectedActivityType);
        });
        model.Should().BeInDescendingOrder(p => p.ActivityCreated);
    }

    [Fact]
    public async Task Index_ReturnsAViewResult_WithCommentedPosts()
    {
        // Arrange
        mockUser.Following.Add(mockFollower);
        mockPostRepo.Setup(repo => repo.GetAllPostsByUser(It.IsAny<string>())).Returns(new List<Post>());
        mockReactionRepo.Setup(repo => repo.GetAllUserComments(It.IsAny<ApplicationUser>())).Returns(mockComments);
        mockReactionRepo.Setup(repo => repo.GetAllUserLikes(It.IsAny<ApplicationUser>())).Returns(new List<Like>());

        var controller = new HomeController(mockPostRepo.Object, mockUserManager.Object, mockUserRepo.Object, mockReactionRepo.Object);

        var expectedHeader = "Test (@Test) commented on";
        var expectedActivityType = ActivityType.Comment;

        // Act
        var result = await controller.Index();

        // Assert
        var model = result.As<ViewResult>().ViewData.Model.As<IEnumerable<ReadPostViewModel>>();
        model.Should().AllSatisfy(p =>
        {
            p.Header.Should().Be(expectedHeader);
            p.ActivityType.Should().Be(expectedActivityType);
        });
        model.Should().BeInDescendingOrder(p => p.ActivityCreated);
    }
}
