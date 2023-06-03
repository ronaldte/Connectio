
namespace Connectio.Tests.Controllers;

public class SearchControllerTests
{
    private readonly Mock<ISearchRepository> mockSearchRepo;
    private readonly ApplicationUser mockUser;
    private readonly List<Post> mockPosts;
    private readonly List<ApplicationUser> mockUsers;
    private readonly List<Tag> mockTags;
    public SearchControllerTests()
    {
        mockSearchRepo = new Mock<ISearchRepository>();
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

        mockUsers = new()
        {
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>(),
            Mock.Of<ApplicationUser>()
        };

        mockTags = new()
        {
            Mock.Of<Tag>(),
            Mock.Of<Tag>(),
            Mock.Of<Tag>(),
            Mock.Of<Tag>(),
            Mock.Of<Tag>(),
            Mock.Of<Tag>()
        };

        mockSearchRepo.Setup(repo => repo.GetPosts(It.IsAny<string>())).Returns(mockPosts);
        mockSearchRepo.Setup(repo => repo.GetTags(It.IsAny<string>())).Returns(mockTags);
        mockSearchRepo.Setup(repo => repo.GetUsers(It.IsAny<string>())).Returns(mockUsers);
    }

    [Fact]
    public void Index_ReturnsAViewResult_WithSearchViewModel()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.Index(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().NotBeNull().And.BeOfType<SearchViewModel>();
    }

    [Fact]
    public void Index_ReturnsAViewResult_WithSearchViewModelTypes()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.Index(It.IsAny<string>());

        // Assert
        var model = result.As<ViewResult>().ViewData.Model.As<SearchViewModel>();
        model.Posts.Should().BeAssignableTo<IEnumerable<ReadPostViewModel>>();
        model.Tags.Should().BeAssignableTo<IEnumerable<ReadTagViewModel>>();
        model.Users.Should().BeAssignableTo<IEnumerable<ReadUserViewModel>>();
    }

    [Fact]
    public void Index_ReturnsAViewResult_WithNoFindings()
    {
        // Arrange
        mockSearchRepo.Setup(repo => repo.GetPosts(It.IsAny<string>())).Returns(new List<Post>());
        mockSearchRepo.Setup(repo => repo.GetTags(It.IsAny<string>())).Returns(new List<Tag>());
        mockSearchRepo.Setup(repo => repo.GetUsers(It.IsAny<string>())).Returns(new List<ApplicationUser>());

        var controller = new SearchController(mockSearchRepo.Object);
        var searchKeyword = "Text";

        // Act
        var result = controller.Index(searchKeyword);

        // Assert
        var model = result.As<ViewResult>().ViewData.Model.As<SearchViewModel>();

        model.SearchKeyword.Should().Be(searchKeyword);

        model.Posts.Should().BeEmpty();
        model.TotalFoundPosts.Should().Be(0);

        model.Tags.Should().BeEmpty();
        model.TotalFoundTags.Should().Be(0);

        model.Users.Should().BeEmpty();
        model.TotalFoundUsers.Should().Be(0);
    }

    [Fact]
    public void Index_ReturnsAViewResult_WithManyFindings()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.Index(It.IsAny<string>());

        // Assert
        var model = result.As<ViewResult>().ViewData.Model.As<SearchViewModel>();
        model.Posts.Should().HaveCount(5);
        model.TotalFoundPosts.Should().Be(mockPosts.Count);
    }

    [Fact]
    public void Tag_ReturnsAViewResult_WithSearchTagViewModel()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.Tag(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<SearchTagViewModel>();
    }

    [Fact]
    public void Tag_ReturnsAViewResult_WithSameSearchKeyword()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);
        var searchKeyword = "TagTest";

        // Act
        var result = controller.Tag(searchKeyword);

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchTagViewModel>().SearchKeyword.Should().Be(searchKeyword);
    }

    [Fact]
    public void Tag_ReturnsAViewResult_WithSearchTagModelTypeInModel()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);
        var searchKeyword = "TagTest";

        // Act
        var result = controller.Tag(searchKeyword);

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchTagViewModel>().Tags.Should().NotBeNull().And.BeAssignableTo<IEnumerable<ReadTagViewModel>>();
    }

    [Fact]
    public void Tag_ReturnsAViewResult_WithEmptyTagList()
    {
        // Arrange
        mockSearchRepo.Setup(repo => repo.GetTags(It.IsAny<string>())).Returns(new List<Tag>());
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.Tag(It.IsAny<string>());

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchTagViewModel>().Tags.Should().BeEmpty();
    }

    [Fact]
    public void Tag_ReturnsAViewResult_WithManyTags()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.Tag(It.IsAny<string>());

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchTagViewModel>().Tags.Should().HaveCount(mockTags.Count);
    }

    [Fact]
    public void Post_ReturnsAViewResult_WithSearchPostViewModel()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.Post(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<SearchPostViewModel>();
    }

    [Fact]
    public void Post_ReturnsAViewResult_WithSameSearchKeyword()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);
        var searchKeyword = "PostTest";

        // Act
        var result = controller.Post(searchKeyword);

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchPostViewModel>().SearchKeyword.Should().Be(searchKeyword);
    }

    [Fact]
    public void Post_ReturnsAViewResult_WithSearchPostModelTypeInModel()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.Post(It.IsAny<string>());

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchPostViewModel>().Posts.Should().NotBeNull().And.BeAssignableTo<IEnumerable<ReadPostViewModel>>();
    }

    [Fact]
    public void Post_ReturnsAViewResult_WithEmptyTagList()
    {
        // Arrange
        mockSearchRepo.Setup(repo => repo.GetPosts(It.IsAny<string>())).Returns(new List<Post>());
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.Post(It.IsAny<string>());

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchPostViewModel>().Posts.Should().BeEmpty();
    }

    [Fact]
    public void Post_ReturnsAViewResult_WithManyPosts()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.Post(It.IsAny<string>());

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchPostViewModel>().Posts.Should().HaveCount(mockPosts.Count);
    }

    [Fact]
    public void UserProfile_ReturnsAViewResult_WithSearchUserViewModel()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.UserProfile(It.IsAny<string>());

        // Assert
        result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<SearchUserViewModel>();
    }

    [Fact]
    public void UserProfile_ReturnsAViewResult_WithSameSearchKeyword()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);
        var searchKeyword = "UserTest";

        // Act
        var result = controller.UserProfile(searchKeyword);

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchUserViewModel>().SearchKeyword.Should().Be(searchKeyword);
    }

    [Fact]
    public void UserProfile_ReturnsAViewResult_WithSearchUserModelTypeInModel()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.UserProfile(It.IsAny<string>());

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchUserViewModel>().Users.Should().NotBeNull().And.BeAssignableTo<IEnumerable<ReadUserViewModel>>();
    }

    [Fact]
    public void UserProfile_ReturnsAViewResult_WithEmptyTagList()
    {
        // Arrange
        mockSearchRepo.Setup(repo => repo.GetUsers(It.IsAny<string>())).Returns(new List<ApplicationUser>());
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.UserProfile(It.IsAny<string>());

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchUserViewModel>().Users.Should().BeEmpty();
    }

    [Fact]
    public void UserProfile_ReturnsAViewResult_WithManyUsers()
    {
        // Arrange
        var controller = new SearchController(mockSearchRepo.Object);

        // Act
        var result = controller.UserProfile(It.IsAny<string>());

        // Arrange
        result.As<ViewResult>().ViewData.Model.As<SearchUserViewModel>().Users.Should().HaveCount(mockUsers.Count);
    }
}
