using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Connectio.Tests.Controllers;

public class ReactionControllerTests
{
    private readonly Mock<IReactionRepository> mockReactionRepo;
    private readonly Mock<IPostRepository> mockPostRepo;
    private readonly Mock<IMentionRepository> mockMentionRepo;
    private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
    private readonly ApplicationUser mockUser = Mock.Of<ApplicationUser>();
    private readonly Post mockPost;
    private readonly List<ApplicationUser> mockUsers;
    private readonly List<Post> mockPosts;
    private readonly List<Bookmark> mockBookmarks;
    public ReactionControllerTests()
    {
        mockReactionRepo = new Mock<IReactionRepository>();
        mockPostRepo = new Mock<IPostRepository>();
        mockMentionRepo = new Mock<IMentionRepository>();
        mockUserManager = MockHelpers.MockUserManager<ApplicationUser>();
        mockPost = new() { Id = 7, Text = "Example7", Created = new DateTime(2023, 04, 10, 10, 0, 0), User = mockUser };
        mockUsers = new List<ApplicationUser>()
        {
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>()
        };
        mockPosts = new()
        {
            new Post() { Id = 1, Text = "Example1", Created = new DateTime(2023, 04, 09, 10, 0, 0), User = mockUser },
            new Post() { Id = 2, Text = "Example2", Created = new DateTime(2023, 04, 09, 9, 0, 0), User = mockUser },
            new Post() { Id = 3, Text = "Example3", Created = new DateTime(2023, 04, 09, 11, 0, 0), User = mockUser},
            new Post() { Id = 4, Text = "Example4", Created = new DateTime(2023, 04, 09, 9, 30, 0), User = mockUser},
            new Post() { Id = 5, Text = "Example5", Created = new DateTime(2023, 04, 09, 11, 10, 0), User = mockUser},
            new Post() { Id = 6, Text = "Example6", Created = new DateTime(2023, 04, 09, 11, 15, 0), User = mockUser}
        };
        mockBookmarks = new()
        {
            new Bookmark() { Post = mockPosts[0], User =  mockUser },
            new Bookmark() { Post = mockPosts[1], User =  mockUser },
            new Bookmark() { Post = mockPosts[2], User =  mockUser }
        };
    }

    [Fact]
    public void DisplayPostLikes_ReturnsNotFound_WhenNoPostExists()
    {
        // Arrange
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(value: null);

        // Act
        var result = controller.DisplayPostLikes(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void DisplayPostLikes_ReturnsAViewResult_WhenPostExists()
    {
        // Arrange
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);

        // Act
        var result = controller.DisplayPostLikes(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<ReadPostLikesViewModel>();
    }

    [Fact]
    public void DisplayPostLikes_ReturnsAViewResult_WithModelData()
    {
        // Arrange
        mockPost.LikedBy.AddRange(mockUsers);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);

        // Act
        var result = controller.DisplayPostLikes(It.IsAny<int>());
        var expectedPost = new ReadPostViewModel(mockPost);
        var expectedLikes = mockUsers.Select(u => new ReadUserViewModel(u));

        // Assert
        var model = result.As<ViewResult>().ViewData.Model.As<ReadPostLikesViewModel>();
        model.Should().NotBeNull();
        model.Post.Should().BeEquivalentTo(expectedPost);
        model.LikedBy.Should().BeEquivalentTo(expectedLikes);
    }

    [Fact]
    public async Task UserBookmarks_ReturnsNotFount_WhenUserNotLoggedIn()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.UserBookmarks();

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task UserBookmarks_ReturnsAViewResult_WithReadAllVBookmarksViewModel()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockReactionRepo.Setup(repo => repo.GetAllBookmarks(It.IsAny<ApplicationUser>())).Returns(new List<Bookmark>());
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.UserBookmarks();

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<ReadAllBookmarksViewModel>();
    }

    [Fact]
    public async Task UserBookmarks_ReturnsAViewResult_WithBookmarksInModel()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockReactionRepo.Setup(repo => repo.GetAllBookmarks(It.IsAny<ApplicationUser>())).Returns(mockBookmarks);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);
        var expectedBookmarkedPosts = new ReadAllBookmarksViewModel(mockBookmarks).BookmarkedPosts;

        // Act
        var result = await controller.UserBookmarks();

        // Assert
        result.As<ViewResult>().ViewData.Model.As<ReadAllBookmarksViewModel>().BookmarkedPosts.Should().BeEquivalentTo(expectedBookmarkedPosts);
    }

    [Fact]
    public async Task CreateBookmark_ReturnsNotFound_WhenNoSuchPostExists()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(value: null);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.CreateBookmark(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateBookmark_ReturnsNotFound_WhenUserNotLoggedIn()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.CreateBookmark(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateBookmark_RedirectsToAction_WhenBookmarkCreated()
    {
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.CreateBookmark(It.IsAny<int>());

        // Assert
        var redirectionToAction = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();

        redirectionToAction.ActionName = "UserBookmarks";
        redirectionToAction.ControllerName.Should().BeNull();
    }

    [Fact]
    public async Task DeleteBookmark_ReturnsNotFound_WhenNoSuchPostExists()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(value: null);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.DeleteBookmark(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteBookmark_ReturnsNotFound_WhenUserNotLoggedIn()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.DeleteBookmark(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteBookmark_ReturnsNotFound_WhenNoSuchBookmarkExists()
    {
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        mockReactionRepo.Setup(repo => repo.GetBookmark(It.IsAny<ApplicationUser>(), It.IsAny<Post>())).Returns(value: null);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.DeleteBookmark(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteBookmark_RedirectsToAction_WhenBookmarkDeleted()
    {
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        mockReactionRepo.Setup(repo => repo.GetBookmark(It.IsAny<ApplicationUser>(), It.IsAny<Post>())).Returns(mockBookmarks.First());
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.DeleteBookmark(It.IsAny<int>());

        // Assert
        var redirectionToAction = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();

        redirectionToAction.ActionName = "UserBookmarks";
        redirectionToAction.ControllerName.Should().BeNull();
    }

    [Fact]
    public async Task CreateLike_ReturnsNotFound_WhenNoSuchPostExists()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(value: null);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.CreateLike(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateLike_ReturnsNotFound_WhenUserNotLoggedIn()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.CreateLike(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateLike_RedirectsToAction_WhenLikeCreated()
    {
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.CreateLike(It.IsAny<int>());

        // Assert
        var redirectionToAction = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();

        redirectionToAction.ActionName.Should().Be("Index");
        redirectionToAction.ControllerName.Should().Be("Home");
    }

    [Fact]
    public async Task DeleteLike_ReturnsNotFound_WhenNoSuchPostExists()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(value: null);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.DeleteLike(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteLike_ReturnsNotFound_WhenUserNotLoggedIn()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.DeleteLike(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteLike_ReturnsNotFound_WhenNoSuchLikeExists()
    {
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        mockReactionRepo.Setup(repo => repo.GetLike(It.IsAny<ApplicationUser>(), It.IsAny<Post>())).Returns(value: null);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.DeleteLike(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task DeleteLike_RedirectsToAction_WhenLikeCreated()
    {
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        mockReactionRepo.Setup(repo => repo.GetLike(It.IsAny<ApplicationUser>(), It.IsAny<Post>())).Returns(new Like());
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.DeleteLike(It.IsAny<int>());

        // Assert
        var redirectionToAction = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();

        redirectionToAction.ActionName.Should().Be("Index");
        redirectionToAction.ControllerName.Should().Be("Home");
    }

    [Fact]
    public void CreateComment_RetursNotFound_WhenNoSuchPostExists()
    {
        // Arrange
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(value: null);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = controller.CreateComment(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void CreateComment_ReturnsAViewResult_WithDisplayCreateCommentViewModel()
    {
        // Arrange
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);
        var expectedResult = new ReadPostViewModel(mockPost);

        // Act
        var result = controller.CreateComment(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<DisplayCreateCommentViewModel>().Which.Post.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task CreateComment_ReturnsANotFound_WhenNoSuchPostExist()
    {
        // Arrage
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(value: null);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.CreateComment(It.IsAny<int>(), It.IsAny<CreateCommentViewModel>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateComment_ReturnsANotFound_WhenNoUserNotLoggedIn()
    {
        // Arrage
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = await controller.CreateComment(It.IsAny<int>(), It.IsAny<CreateCommentViewModel>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task CreateComment_ReturnsAViewResult_WhenInvalidModelState()
    {
        // Arrage
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);
        controller.ModelState.AddModelError("Text", "Required");
        var viewModel = new CreateCommentViewModel() { Text = string.Empty };
        var expectedResult = new DisplayCreateCommentViewModel(mockPost) { Text = viewModel.Text };

        // Act
        var result = await controller.CreateComment(It.IsAny<int>(), viewModel);

        // Assert
        var model = result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<DisplayCreateCommentViewModel>().Which.As<DisplayCreateCommentViewModel>();
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public async Task CreateComment_RedirectsToAction_WhenCommentCreated()
    {
        // Arrage
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockPostRepo.Setup(repo => repo.GetPostById(It.IsAny<int>())).Returns(mockPost);
        var viewModel = new CreateCommentViewModel() { Text = "Test" };
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);
        var expectedRouteValue = new RouteValueDictionary()
        {
            { "id", mockPost.Id}
        };

        // Act
        var result = await controller.CreateComment(mockPost.Id, viewModel);

        // Assert
        var redirection = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();
        redirection.ControllerName.Should().NotBeNull().And.Be("Post");
        redirection.ActionName.Should().NotBeNull().And.Be("Index");
        redirection.RouteValues.Should().BeEquivalentTo(expectedRouteValue);
    }

    [Fact]
    public void PostsWithTag_ReturnsANotFound_WhenNoSuchTagExists()
    {
        // Arrage
        mockMentionRepo.Setup(repo => repo.GetTag(It.IsAny<string>())).Returns(value: null);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = controller.PostsWithTag(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public void PostsWithTag_ReturnsAViewResult_WithReadPostsWithTagViewModel()
    {
        // Arrange
        mockMentionRepo.Setup(repo => repo.GetTag(It.IsAny<string>())).Returns(new Tag());
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        // Act
        var result = controller.PostsWithTag(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<ReadPostsWithTagViewModel>();
    }

    [Fact]
    public void PostsWithTag_ReturnsAViewResult_WithDataInViewModel()
    {
        // Arrange
        var mockTag = new Tag() { Id = 1, Name = "Test", Posts = mockPosts };
        mockMentionRepo.Setup(repo => repo.GetTag(It.IsAny<string>())).Returns(mockTag);
        mockMentionRepo.Setup(repo => repo.GetPosts(It.IsAny<Tag>())).Returns(mockPosts);
        var controller = new ReactionController(mockReactionRepo.Object, mockPostRepo.Object, mockMentionRepo.Object, mockUserManager.Object);

        var expectedTag = new ReadTagViewModel(mockTag);
        var expectedPosts = mockPosts.Select(p => new ReadPostViewModel(p));

        // Act
        var result = controller.PostsWithTag(It.IsAny<string>());

        // Assert
        var model = result.As<ViewResult>().ViewData.Model.As<ReadPostsWithTagViewModel>();
        model.Tag.Should().NotBeNull().And.BeEquivalentTo(expectedTag);
        model.Posts.Should().NotBeNull().And.BeEquivalentTo(expectedPosts);
    }
}