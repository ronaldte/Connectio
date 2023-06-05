
using Microsoft.AspNetCore.Routing;
using System.Security.Claims;

namespace Connectio.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserRepository> mockUserRepo;
    private readonly Mock<IPostRepository> mockPostRepo;
    private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
    private readonly ApplicationUser mockUser;
    private readonly List<Post> mockPosts;
    public UserControllerTests()
    {
        mockUserRepo = new Mock<IUserRepository>();
        mockPostRepo = new Mock<IPostRepository>();
        mockUserManager = MockHelpers.MockUserManager<ApplicationUser>();
        mockUser = Mock.Of<ApplicationUser>();
        mockPosts = new()
        {
            new Post() { Id = 1, Text = "Example1", Created = new DateTime(2023, 04, 09, 10, 0, 0), User = mockUser },
            new Post() { Id = 2, Text = "Example2", Created = new DateTime(2023, 04, 09, 9, 0, 0), User = mockUser },
            new Post() { Id = 3, Text = "Example3", Created = new DateTime(2023, 04, 09, 11, 0, 0), User = mockUser},
            new Post() { Id = 4, Text = "Example4", Created = new DateTime(2023, 04, 09, 9, 30, 0), User = mockUser},
            new Post() { Id = 5, Text = "Example5", Created = new DateTime(2023, 04, 09, 11, 10, 0), User = mockUser},
            new Post() { Id = 6, Text = "Example6", Created = new DateTime(2023, 04, 09, 11, 15, 0), User = mockUser}
        };

        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);
        mockPostRepo.Setup(repo => repo.GetAllPostsByUser(It.IsAny<string>())).Returns(mockPosts);
    }

    [Fact]
    public void Index_ReturnsNotFound_WhenNoUserExists()
    {
        // Arrange
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(value: null);

        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);

        // Act
        var result = controller.Index(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Index_ReturnsAViewResult_WhenUserExists()
    {
        // Assert
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);

        // Act
        var result = controller.Index(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<ReadUserViewModel>();
    }

    [Fact]
    public void Index_ReturnsAViewResult_WithSameUser()
    {
        // Asert
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        var userViewModel = new ReadUserViewModel(mockUser, mockPosts.Select(p => new ReadPostViewModel(p)));

        // Act
        var result = controller.Index(It.IsAny<string>());

        // Assert
        result.As<ViewResult>().ViewData.Model.As<ReadUserViewModel>().Should().BeEquivalentTo(userViewModel);
    }

    [Fact]
    public void Index_ReturnsAViewResult_WithNoPosts()
    {
        // Arrange
        mockPostRepo.Setup(repo => repo.GetAllPostsByUser(It.IsAny<string>())).Returns(new List<Post>());
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        var userViewModel = new ReadUserViewModel(mockUser, new List<ReadPostViewModel>());

        // Act
        var result = controller.Index(It.IsAny<string>());

        // Assert
        result.As<ViewResult>().ViewData.Model.As<ReadUserViewModel>().Should().BeEquivalentTo(userViewModel);
    }

    [Fact]
    public void Index_ReturnsAViewResult_WithMultipleOrderedPosts()
    {
        // Arrage
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        var userViewModel = new ReadUserViewModel(mockUser, mockPosts.OrderByDescending(p => p.Created).Select(p => new ReadPostViewModel(p)));

        // Act
        var result = controller.Index(It.IsAny<string>());

        // Assert
        result.As<ViewResult>().ViewData.Model.As<ReadUserViewModel>().Should().BeEquivalentTo(userViewModel);
    }

    [Fact]
    public async Task AddFollower_ReturnsNotFound_WhenNoUserToFollowExists()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(value: null);

        // Act
        var result = await controller.AddFollower(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task AddFollower_ReturnsNotFound_WhenUserNotLoggedIn()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser).Verifiable();
        mockUserManager.Setup(repo => repo.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null).Verifiable();

        // Act
        var result = await controller.AddFollower(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        mockUserRepo.Verify(repo => repo.GetUserByUserName(It.IsAny<string>()));
        mockUserManager.Verify(repo => repo.GetUserAsync(It.IsAny<ClaimsPrincipal>()));
    }
    [Fact]
    public async Task AddFollower_ReturnsBadRequest_WhenFollowingSelf()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);
        mockUserManager.Setup(repo => repo.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);

        // Act
        var result = await controller.AddFollower(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task AddFollower_RedirectsToAction_WhenStartedFollowing()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        var username = "TextFollowing";
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);
        mockUserManager.Setup(repo => repo.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(Mock.Of<ApplicationUser>());
        var expectedRouteValues = new RouteValueDictionary
        {
            { "username", username },
        };

        // Act
        var result = await controller.AddFollower(username);

        // Assert
        var redirection = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();
        redirection.Should().NotBeNull();
        redirection.ActionName.Should().Be("Index");
        redirection.ControllerName.Should().Be("User");
        redirection.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
    }

    [Fact]
    public async Task DeleteFollower_ReturnsNotFound_WhenNoUserToFollowExists()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(value: null);

        // Act
        var result = await controller.DeleteFollower(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteFollower_ReturnsNotFound_WhenUserNotLoggedIn()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser).Verifiable();
        mockUserManager.Setup(repo => repo.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null).Verifiable();

        // Act
        var result = await controller.DeleteFollower(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
        mockUserRepo.Verify(repo => repo.GetUserByUserName(It.IsAny<string>()));
        mockUserManager.Verify(repo => repo.GetUserAsync(It.IsAny<ClaimsPrincipal>()));
    }
    [Fact]
    public async Task DeleteFollower_ReturnsBadRequest_WhenFollowingSelf()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);
        mockUserManager.Setup(repo => repo.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);

        // Act
        var result = await controller.DeleteFollower(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task DeleteFollower_RedirectsToAction_WhenStartedFollowing()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        var username = "TextFollowing";
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);
        mockUserManager.Setup(repo => repo.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(Mock.Of<ApplicationUser>());
        var expectedRouteValues = new RouteValueDictionary
        {
            { "username", username },
        };

        // Act
        var result = await controller.DeleteFollower(username);

        // Assert
        var redirection = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();
        redirection.Should().NotBeNull();
        redirection.ActionName.Should().Be("Index");
        redirection.ControllerName.Should().Be("User");
        redirection.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
    }

    [Fact]
    public void Followers_ReturnsNotFound_WhenNoUserExists()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(value: null);

        // Act
        var result = controller.Followers(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Followers_ReturnsAViewResult_WithFollowersViewModel()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);

        // Act
        var result = controller.Followers(It.IsAny<string>());

        // Assert
        var model = result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<DisplayFollowerFollowingViewModel>().Which.As<DisplayFollowerFollowingViewModel>();
        model.User.Should().BeOfType<ReadUserViewModel>();
        model.Users.Should().BeAssignableTo<IEnumerable<ReadUserViewModel>>();
    }

    [Fact]
    public void Followers_ReturnsAViewResult_WithFollowers()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);

        var followers = new List<ApplicationUser>()
        {
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>()
        };
        mockUser.Followers.AddRange(followers);

        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);

        var expectedResult = followers.Select(u => new ReadUserViewModel(u));

        // Act
        var result = controller.Followers(It.IsAny<string>());

        // Assert
        var model = result.As<ViewResult>().ViewData.Model.As<DisplayFollowerFollowingViewModel>();
        model.Users.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Following_ReturnsNotFound_WhenNoUserExists()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(value: null);

        // Act
        var result = controller.Following(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Following_ReturnsAViewResult_WithFollowersFollowingViewModel()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);

        // Act
        var result = controller.Following(It.IsAny<string>());

        // Assert
        var model = result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<DisplayFollowerFollowingViewModel>().Which.As<DisplayFollowerFollowingViewModel>();
        model.User.Should().BeOfType<ReadUserViewModel>();
        model.Users.Should().BeAssignableTo<IEnumerable<ReadUserViewModel>>();
    }

    [Fact]
    public void Following_ReturnsAViewResult_WithFollowings()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);

        var followings = new List<ApplicationUser>()
        {
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>()
        };
        mockUser.Following.AddRange(followings);

        mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUser);

        var expectedResult = followings.Select(u => new ReadUserViewModel(u));

        // Act
        var result = controller.Following(It.IsAny<string>());

        // Assert
        var model = result.As<ViewResult>().ViewData.Model.As<DisplayFollowerFollowingViewModel>();
        model.Users.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void UpdateProfilePicture_ReturnsAViewResult()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);

        // Act
        var result = controller.UpdateProfilePicture();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task UpdateProfilePicture_ReturnsAViewResult_WhenModelInvalid()
    {
        // Arrange 
        var viewModel = new CreateFileViewModel();
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        controller.ModelState.AddModelError("File", "Required");

        // Act
        var result = await controller.UpdateProfilePicture(viewModel);

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeNull();
    }

    [Fact]
    public void UpdateBannerPicture_ReturnsAViewResult()
    {
        // Arrange
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);

        // Act
        var result = controller.UpdateBannerPicture();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task UpdateBannerPicture_ReturnsAViewResult_WhenModelInvalid()
    {
        // Arrange 
        var viewModel = new CreateFileViewModel();
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);
        controller.ModelState.AddModelError("File", "Required");

        // Act
        var result = await controller.UpdateBannerPicture(viewModel);

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeNull();
    }

    [Fact]
    public async Task RemoveProfilePicture_RedirectsToAction_WhenPictureUpdated()
    {
        // Arrange
        mockUserManager.Setup(repo => repo.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockUserRepo.Setup(repo => repo.UpdateProfilePicture(It.IsAny<ApplicationUser>(), It.IsAny<string>()));
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.RemoveProfilePicture();

        // Assert
        var redirection = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();
        redirection.Should().NotBeNull();
        redirection.ActionName.Should().Be("UpdateProfilePicture");
        redirection.ControllerName.Should().BeNull();
    }

    [Fact]
    public async Task RemoveBannerPicture_RedirectsToAction_WhenPictureUpdated()
    {
        // Arrange
        mockUserManager.Setup(repo => repo.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockUserRepo.Setup(repo => repo.UpdateBannerPicture(It.IsAny<ApplicationUser>(), It.IsAny<string>()));
        var controller = new UserController(mockUserRepo.Object, mockPostRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.RemoveBannerPicture();

        // Assert
        var redirection = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();
        redirection.Should().NotBeNull();
        redirection.ActionName.Should().Be("UpdateBannerPicture");
        redirection.ControllerName.Should().BeNull();
    }
}
