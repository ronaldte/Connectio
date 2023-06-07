
using Connectio.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Connectio.Tests.Controllers;

public class ConversationControllerTests
{
	private readonly Mock<IUserRepository> mockUserRepo;
	private readonly Mock<IConversationRepository> mockConversationRepo;
	private readonly Mock<UserManager<ApplicationUser>> mockUserManager;
	private readonly Mock<IHubContext<MessageHub>> mockMessageHub;
	private readonly ConversationController controller;
	private readonly ApplicationUser mockUser;
	private readonly ApplicationUser mockUserToMessage;
	private readonly Conversation mockConversation;
	private readonly Message mockMessage;
	private readonly Message mockLastMessage;

	public ConversationControllerTests()
	{
		mockUserRepo = new Mock<IUserRepository>();
		mockConversationRepo = new Mock<IConversationRepository>();
		mockUserManager = MockHelpers.MockUserManager<ApplicationUser>();
		mockMessageHub = new Mock<IHubContext<MessageHub>>();

		controller = new ConversationController(mockUserRepo.Object, mockConversationRepo.Object, mockUserManager.Object, mockMessageHub.Object);

		mockUser = Mock.Of<ApplicationUser>();
		mockUserToMessage = Mock.Of<ApplicationUser>();

		mockConversation = new Conversation() { Id = 1, IsPrivate = true, Participants = new List<ApplicationUser>() { mockUser, mockUserToMessage } };
		mockMessage = new Message() { Conversation = mockConversation, CreatedAt = new DateTime(2023, 04, 10, 11, 0, 0), CreatedBy = mockUser, Id = 1, Text = "Test" };
		mockConversation.Messages.Add(mockMessage);
		mockLastMessage = new Message() { Conversation = mockConversation, CreatedAt = new DateTime(2023, 04, 10, 13, 0, 0), CreatedBy = mockUser, Id = 2, Text = "LastMessage" };
		mockConversation.Messages.Add(mockLastMessage);

	}

	[Fact]
	public async Task Create_ReturnsAViewResult_WhenUserNameIsEmpty()
	{
		// Arrange
		var userName = string.Empty;

		// Act
		var result = await controller.Create(userName);

		// Assert
		result.Should().BeOfType<ViewResult>();
	}

	[Fact]
	public async Task Create_ThrowsAnException_WhenUserNotLoggedIn()
	{
		// Arrange
		mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);

		// Act
		Func<Task> act = controller.Awaiting(c => c.Create(username: "Test"));

		// Assert
		await act.Should().ThrowAsync<UnauthorizedAccessException>();
	}

	[Fact]
	public async Task Create_ReturnsAViewResult_WhenNoSuchUserExists()
	{
		// Arrange
		mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
		mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(value: null);

		var userName = "TestUserToMessage";

		// Act
		var result = await controller.Create(userName);

		// Assert
		var model = result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<CreateConversationViewModel>().Which.As<CreateConversationViewModel>();
		model.ToUser.Should().Be(userName);
		result.As<ViewResult>().ViewData.ModelState.ErrorCount.Should().Be(1);
	}

	[Fact]
	public async Task Create_ReturnsAViewResult_WhenUserExists()
	{
		// Arrange
		mockConversationRepo.Setup(repo => repo.GetConversation(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>())).Returns(value: null);
		mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
		mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUserToMessage);

		var userName = "TestUserToMessage";

		// Act
		var result = await controller.Create(userName);

		// Assert
		var model = result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<CreateConversationViewModel>().Which.As<CreateConversationViewModel>();
		model.ToUser.Should().Be(userName);
		result.As<ViewResult>().ViewData.ModelState.ErrorCount.Should().Be(0);
	}

	[Fact]
	public async Task Create_RedirectsToAction_WhenConversationExists()
	{
		// Arrange
		mockConversationRepo.Setup(repo => repo.GetConversation(It.IsAny<ApplicationUser>(), It.IsAny<ApplicationUser>())).Returns(mockConversation);
		mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
		mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUserToMessage);

		var userName = "TestUserToMessage";
		var expectedRouteValues = new RouteValueDictionary()
		{
			{ "conversationId", mockConversation.Id }
		};

		// Act
		var result = await controller.Create(userName);

		// Assert
		var redirection = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();

		redirection.ControllerName.Should().BeNull();
		redirection.ActionName.Should().Be("Read");
		redirection.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
	}

	[Fact]
	public async Task Create_ReturnsAViewResult_WhenInvalidModel()
	{
		// Arrange
		var viewModel = new CreateConversationViewModel() { MessageText = string.Empty };
		controller.ModelState.AddModelError("Text", "Required");

		// Act
		var result = await controller.Create(viewModel);

		// Assert
		result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<CreateConversationViewModel>().Which.Should().BeSameAs(viewModel);
	}

	[Fact]
	public async Task Create_ReturnsAViewResult_WhenNoSuchUserExists_PostMethod()
	{
		// Arrange
		mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(value: null);
		var viewModel = new CreateConversationViewModel() { MessageText = string.Empty, ToUser = "DoesNotExist" };

		// Act
		var result = await controller.Create(viewModel);

		// Assert
		result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<CreateConversationViewModel>().Which.Should().BeSameAs(viewModel);
		result.As<ViewResult>().ViewData.ModelState.ErrorCount.Should().Be(1);
	}

	[Fact]
	public async Task Create_ThrowsAnException_WhenUserNotLoggedIn_PostMethod()
	{
		// Arrange
		mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUserToMessage);
		mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);
		var viewModel = new CreateConversationViewModel() { MessageText = string.Empty, ToUser = "DoesNotExist" };

		// Act
		Func<Task> act = controller.Awaiting(c => c.Create(viewModel));

		// Assert
		await act.Should().ThrowAsync<UnauthorizedAccessException>();
	}

	[Fact]
	public async Task Create_RedirectsToAction_WhenConversationCreated()
	{
		// Arrange
		mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
		mockUserRepo.Setup(repo => repo.GetUserByUserName(It.IsAny<string>())).Returns(mockUserToMessage);
		var viewModel = new CreateConversationViewModel() { MessageText = string.Empty, ToUser = "DoesNotExist" };

		// Act
		var result = await controller.Create(viewModel);

		// Assert
		var redirection = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();

		redirection.ControllerName.Should().BeNull();
		redirection.ActionName.Should().Be("Read");
	}

	[Fact]
	public async Task List_ThrowsAnException_WhenUserNotLoggedIn()
	{
		// Arrange
		mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);

		// Act
		Func<Task> act = controller.Awaiting(c => c.List());

		// Assert
		await act.Should().ThrowAsync<UnauthorizedAccessException>();
	}

	[Fact]
	public async Task List_ReturnsAViewResult_WithNoConversation()
	{
		// Arrange
		mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
		mockConversationRepo.Setup(repo => repo.GetUserConversations(It.IsAny<ApplicationUser>())).Returns(new List<Conversation>());

		// Act
		var result = await controller.List();

		// Assert
		result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeAssignableTo<IEnumerable<DisplayConversationListViewModel>>().Which.Should().BeEmpty();
	}

	[Fact]
	public async Task List_ReturnsAViewResult_WithSingleConversation()
	{
		// Arrange
		mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
		mockConversationRepo.Setup(repo => repo.GetUserConversations(It.IsAny<ApplicationUser>())).Returns(new List<Conversation>() { mockConversation });
		mockConversationRepo.Setup(repo => repo.GetMessages(It.IsAny<int>(), It.IsAny<int>())).Returns(mockConversation.Messages.TakeLast(1));
		var expectedResult = new DisplayConversationListViewModel()
		{
			Id = mockConversation.Id,
			IsPrivate = mockConversation.IsPrivate,
			Participants = new List<ReadUserViewModel>() { new ReadUserViewModel(mockUserToMessage) },
			LastMessage = mockLastMessage
		};

		// Act
		var result = await controller.List();

		// Assert
		var model = result.Should().BeOfType<ViewResult>()
			.Which.ViewData.Model.Should().BeAssignableTo<IEnumerable<DisplayConversationListViewModel>>()
			.Which.As<IEnumerable<DisplayConversationListViewModel>>();
		model.Should().HaveCount(1);
		var conversation = model.First();
		conversation.Should().BeEquivalentTo(expectedResult);
	}

	[Fact]
	public async Task Read_ThrowsAnException_WhenUserNotLoggedIn()
	{
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);

        // Act
        Func<Task> act = controller.Awaiting(c => c.Read(It.IsAny<int>()));

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

	[Fact]
	public async Task Read_ReturnsNotFound_WhenNoSuchConversationExists()
	{
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
		mockConversationRepo.Setup(repo => repo.ConversationExists(It.IsAny<int>())).Returns(false);

		// Act
		var result = await controller.Read(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Read_ReturnsUnauthorized_WhenUserNotInConversation()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockConversationRepo.Setup(repo => repo.ConversationExists(It.IsAny<int>())).Returns(true);
		mockConversationRepo.Setup(repo => repo.GetUserConversations(It.IsAny<ApplicationUser>())).Returns(new List<Conversation>());

        // Act
        var result = await controller.Read(It.IsAny<int>());

        // Assert
        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task Read_ReturnsAViewResult_WithConversationMessages()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockConversationRepo.Setup(repo => repo.ConversationExists(It.IsAny<int>())).Returns(true);
        mockConversationRepo.Setup(repo => repo.GetUserConversations(It.IsAny<ApplicationUser>())).Returns(new List<Conversation>() { mockConversation });
		mockConversationRepo.Setup(repo => repo.GetMessages(It.IsAny<int>(), It.IsAny<int>())).Returns(mockConversation.Messages.TakeLast(10));

		var expectedResult = new DisplayConversationViewModel()
		{
			Id = mockConversation.Id,
			IsPrivate = mockConversation.IsPrivate,
			Participants = new List<ReadUserViewModel>() { new ReadUserViewModel(mockUserToMessage) },
			Messages = mockConversation.Messages.TakeLast(10).Select(m => new ReadMessageViewModel(m)).ToList()
		};

        // Act
        var result = await controller.Read(1);

		// Assert
		result.Should().BeOfType<ViewResult>().Which.ViewData.Model.Should().BeOfType<DisplayConversationViewModel>().Which.Should().BeEquivalentTo(expectedResult);
    }

	[Fact]
	public async Task Send_RedirectsToAction_WhenInvalidModel()
	{
		// Arrange
		controller.ModelState.AddModelError("ConversationId", "Required");

		var viewModel = new CreateMessageViewModel();

		// Act
		var result = await controller.Send(viewModel);

		// Assert
		var redirect = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();
		redirect.Should().NotBeNull();
		redirect.ActionName.Should().Be("List");
		redirect.ControllerName.Should().BeNull();
	}

	[Fact]
	public async Task Send_ThrowsAnException_WhenUserNotLoggedIn()
	{
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(value: null);
        var viewModel = new CreateMessageViewModel();

        // Act
        Func<Task> act = controller.Awaiting(c => c.Send(viewModel));

        // Assert
        await act.Should().ThrowAsync<UnauthorizedAccessException>();
    }

	[Fact]
	public async Task Send_ReturnsNotFound_WhenNoSuchConversationExists()
	{
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockConversationRepo.Setup(repo => repo.ConversationExists(It.IsAny<int>())).Returns(false);
        
		var viewModel = new CreateMessageViewModel();

        // Act
        var result = await controller.Send(viewModel);

		// Assert
		result.Should().BeOfType<NotFoundResult>();
    }

	[Fact]
	public async Task Send_ReturnsUnauthorized_WhenUserNotInConversation()
	{
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockConversationRepo.Setup(repo => repo.ConversationExists(It.IsAny<int>())).Returns(true);
        mockConversationRepo.Setup(repo => repo.GetUserConversations(It.IsAny<ApplicationUser>())).Returns(new List<Conversation>());

        var viewModel = new CreateMessageViewModel();

        // Act
        var result = await controller.Send(viewModel);

        // Assert
        result.Should().BeOfType<UnauthorizedResult>();
    }

    [Fact]
    public async Task Send_RedirectsToAction_WhenMessageCreated()
    {
        // Arrange
        mockUserManager.Setup(mgr => mgr.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(mockUser);
        mockConversationRepo.Setup(repo => repo.ConversationExists(It.IsAny<int>())).Returns(true);
        mockConversationRepo.Setup(repo => repo.GetUserConversations(It.IsAny<ApplicationUser>())).Returns(new List<Conversation>() { mockConversation });

		var expectedRouteValues = new RouteValueDictionary()
		{
			{ "conversationId", mockConversation.Id }
		};

        var viewModel = new CreateMessageViewModel() { ConversationId = 1, MessageText = "Test" };

        // Act
        var result = await controller.Send(viewModel);

        // Assert
        var redirection = result.Should().BeOfType<RedirectToActionResult>().Which.As<RedirectToActionResult>();

		redirection.ActionName.Should().Be("Read");
		redirection.ControllerName.Should().BeNull();
		redirection.RouteValues.Should().BeEquivalentTo(expectedRouteValues);
    }
}
