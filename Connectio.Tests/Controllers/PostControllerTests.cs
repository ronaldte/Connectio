using Connectio.ViewModels;
using FluentAssertions;
using System.Security.Claims;

namespace Connectio.Tests.Controllers;

public class PostControllerTests
{
    private readonly Mock<IPostRepository> mockPostRepo;
    private readonly Mock<IMentionRepository> mockMentionRepo;
    private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
    private readonly ApplicationUser mockUser;
    private readonly List<Comment> mockComments;
    private readonly Post mockPost;

    public PostControllerTests()
    {
        mockPostRepo = new Mock<IPostRepository>();
        mockMentionRepo = new Mock<IMentionRepository>();
        mockUserManager = MockHelpers.MockUserManager<ApplicationUser>();
        mockUser = Mock.Of<ApplicationUser>();
        mockComments = new List<Comment>()
        {
            new Comment(){ Id = 1, Text="Comment1", Created = new DateTime(2023, 04, 10, 11, 0, 0), User = mockUser },
            new Comment(){ Id = 2, Text="Comment2", Created = new DateTime(2023, 04, 10, 12, 0, 0), User = mockUser },
            new Comment(){ Id = 3, Text="Comment3", Created = new DateTime(2023, 04, 10, 10, 0, 0), User = mockUser},
        };
        mockPost = new() { Id = 1, Text = "Example1", Created = new DateTime(2023, 04, 09, 10, 0, 0), User = mockUser, Comments = mockComments };
    }

    [Fact]
    public void Index_ReturnsNotFound_WhenNoPostExists()
    {
        // Arrange
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(value: null);
        var controller = new PostController(mockPostRepo.Object, mockUserManager.Object, mockMentionRepo.Object);

        // Act
        var result = controller.Index(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void Index_ReturnsAViewResult_WhenPostExists()
    {
        // Arrange
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new PostController(mockPostRepo.Object, mockUserManager.Object, mockMentionRepo.Object);

        // Act
        var result = controller.Index(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<ViewResult>();

    }

    [Fact]
    public void Index_ReturnsAViewResult_WithReadPostViewModel()
    {
        // Arrange
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new PostController(mockPostRepo.Object, mockUserManager.Object, mockMentionRepo.Object);
        var expectedResult = new ReadPostViewModel(mockPost);

        // Act
        var result = controller.Index(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<ViewResult>()
            .Which.ViewData.Model.Should().BeAssignableTo<ReadPostViewModel>()
            .And
            .BeEquivalentTo(expectedResult, options => options.Excluding(o => o.Comments));
    }

    [Fact]
    public void Index_ReturnsAViewResult_WithPostAndOrderedComments()
    {
        // Arrange
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new PostController(mockPostRepo.Object, mockUserManager.Object, mockMentionRepo.Object);
        var expectedComments = mockComments.OrderByDescending(c => c.Created).Select(c => new ReadCommentViewModel(c));
        var expectedResult = new ReadPostViewModel(mockPost){ Comments = expectedComments };

        // Act
        var result = controller.Index(It.IsAny<int>());
        var viewResult = result.As<ViewResult>();
        var model = viewResult.ViewData.Model.As<ReadPostViewModel>();

        // Assert            
        model.Should().BeEquivalentTo(expectedResult);
        model.Comments.Should().BeEquivalentTo(expectedComments).And.HaveCount(3);
    }

    [Fact]
    public void Create_ReturnsAViewResult()
    {
        // Arrange
        var controller = new PostController(mockPostRepo.Object, mockUserManager.Object, mockMentionRepo.Object);

        // Act
        var result = controller.Create();

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task Create_ReturnsAViewResult_WhenModelStateInvalid()
    {
        // Arrange
        var viewModel = new CreatePostViewModel();
        var controller = new PostController(mockPostRepo.Object, mockUserManager.Object, mockMentionRepo.Object);
        controller.ModelState.AddModelError("Text", "Required");
        
        // Act
        var result = await controller.Create(viewModel);

        // Assert
        result.Should().BeOfType<ViewResult>()
            .Which.ViewData.Model.Should().BeAssignableTo<CreatePostViewModel>().And.BeSameAs(viewModel);
    }

    [Fact]
    public async Task Create_ThrowsAnException_WhenUserNotLoggedIn()
    {
        // Arange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);
        var controller = new PostController(mockPostRepo.Object, mockUserManager.Object, mockMentionRepo.Object);
        var viewModel = new CreatePostViewModel(){ Text = "Test" };

        // Act
        Func<Task> act = controller.Awaiting(c => c.Create(viewModel));

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

    [Fact]
    public async Task Create_RedirectsToAction_WhenPostCreated()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        var controller = new PostController(mockPostRepo.Object, mockUserManager.Object, mockMentionRepo.Object);
        var viewModel = new CreatePostViewModel() { Text = "Test" };

        // Act
        var result = await controller.Create(viewModel);

        // Assert
        result.Should().BeOfType<RedirectToActionResult>();

        var redirectToActionResult = result.As<RedirectToActionResult>();
        redirectToActionResult.ControllerName.Should().BeNull();
        redirectToActionResult.ActionName.Should().Be("Index");
    }
}
