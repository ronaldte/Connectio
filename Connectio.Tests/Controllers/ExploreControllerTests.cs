

namespace Connectio.Tests.Controllers;

public class ExploreControllerTests
{
    private readonly Mock<IPostRepository> mockPostRepo;
    private readonly ApplicationUser mockUser;
    private readonly List<Post> mockPosts;

    public ExploreControllerTests()
    {
        mockPostRepo = new Mock<IPostRepository>();
        mockUser = Mock.Of<ApplicationUser>();
        mockPosts = new() 
        {
            new Post() { Id = 1, Text = "Example1", Created = new DateTime(2023, 04, 09, 10, 0, 0), User = mockUser },
            new Post() { Id = 2, Text = "Example2", Created = new DateTime(2023, 04, 09, 9, 0, 0), User = mockUser },
            new Post() { Id = 3, Text = "Example3", Created = new DateTime(2023, 04, 09, 11, 0, 0), User = mockUser}
        };
    }
    
    [Fact]
    public void Index_ReturnsAViewResult_WithAListOfPosts()
    {
        // Arrange
        mockPostRepo.Setup(repo => repo.GetAllPosts()).Returns(mockPosts);
        var controller = new ExploreController(mockPostRepo.Object);

        // Act
        var result = controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>()
            .Which.ViewData.Model.Should().BeAssignableTo<IEnumerable<ReadPostViewModel>>()
            .Which.Should().HaveCount(3);
            
    }

    [Fact]
    public void Index_ReturnsAViewResult_WithAnEmptyListOfPosts()
    {
        // Arrange
        mockPostRepo.Setup(repo => repo.GetAllPosts()).Returns(new List<Post>());
        var controller = new ExploreController(mockPostRepo.Object);

        // Act
        var result = controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>()
            .Which.ViewData.Model.Should().BeAssignableTo<IEnumerable<ReadPostViewModel>>()
            .Which.Should().BeEmpty();
    }

    [Fact]
    public void Index_ReturnsAViewResult_WithAnOrderedListOfPosts()
    {
        // Arrange
        mockPostRepo.Setup(repo => repo.GetAllPosts()).Returns(mockPosts);
        var controller = new ExploreController(mockPostRepo.Object);
        var expectedResult = mockPosts.OrderByDescending(p => p.Created).Select(p => new ReadPostViewModel(p));

        // Act
        var result = controller.Index();

        // Assert
        result.Should().BeOfType<ViewResult>()
            .Which.ViewData.Model.Should().BeAssignableTo<IEnumerable<ReadPostViewModel>>()
            .Which.Should().BeEquivalentTo(expectedResult);
    }
}
