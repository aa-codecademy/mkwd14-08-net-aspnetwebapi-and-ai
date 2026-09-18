using Microsoft.Extensions.Logging;
using Moq;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.Dtos;
using NotesApp.Services.CustomExceptions;
using NotesApp.Services.Implementations;

namespace NotesApp.Tests;

// ===> Naming Convention
// MethodName_StateUnderTest_ExpectedBehavior

// ===> AAA => Arange - Act - Assert
// *Arrange* => Set up the test by preparing the objects, mocking dependencies, and setting the initial state.
// *Act* => Execute the action or method that is being tested.
// *Assert* =>  Verify that the outcome from the action is as expected, confirming that the method works correctly.

// ===> Mocking is a technique used in unit testing to create fake objects

[TestClass]
public class NoteServiceTests
{
    private Mock<INoteRepository> _noteRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ITagRepository> _tagRepositoryMock;
    private Mock<ILogger<NoteService>> _logger;

    private NoteService _noteService;

    private const int UserId = 100;

    [TestInitialize]
    public void Setup()
    {
        _noteRepositoryMock = new Mock<INoteRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _logger = new Mock<ILogger<NoteService>>();

        _noteService = new NoteService(
            _noteRepositoryMock.Object,
            _userRepositoryMock.Object,
            _tagRepositoryMock.Object,
            _logger.Object
        );
    }

    #region GetAllNotesAsync

    [TestMethod]
    public async Task GetAllNotesAsync_WhenNoPriorityIsGiven_ReturnsEntitiesAndMapsThem()
    {
        // Arrange
        string noteText = "Testing with MSTest";
        Note note = CreateNote(5, UserId, noteText, Priority.High);
        List<Note> notesDb = [note];

        _noteRepositoryMock.Setup(repo => repo.GetAllAsync(UserId)).ReturnsAsync(notesDb);

        // Act
        List<NoteDto> result = await _noteService.GetAllNotesAsync(UserId);

        // Assert 
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual(noteText, result[0].Text);
        Assert.AreEqual(note.User.FullName, result[0].UserFullName);
    }

    [TestMethod]
    public async Task GetAllNotesAsync_WhenPriorityIsGiven_UsesTheFilteringQuery()
    {
        // Arrange
        _noteRepositoryMock
            .Setup(repo => repo.GetAllByPriorityAsync(UserId, Priority.High))
            .ReturnsAsync(new List<NoteDto>
            {
                new NoteDto { Id = 5, Text = "Buy milk", Priority = Priority.High }
            });

        // Act
        List<NoteDto> result = await _noteService.GetAllNotesAsync(UserId, Priority.High);

        // Assert
        Assert.AreEqual(1, result.Count);
        _noteRepositoryMock.Verify(
            repo => repo.GetAllAsync(It.IsAny<int>()),
            Times.Never);
    }

    #endregion

    #region GetNoteByIdAsync

    [TestMethod]
    public async Task GetNoteByIdAsync_WhenNoteIsNotFound_ThrowsNoteNotFoundException()
    {
        // Arrange
        int noteId = 1;
        string errorMessage = $"Note with Id {noteId} not found.";
        _noteRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Note?)null);

        // Act & Assert
        NoteNotFoundException result = await Assert.ThrowsExactlyAsync<NoteNotFoundException>(() => _noteService.GetNoteByIdAsync(noteId, UserId));
        Assert.AreEqual(errorMessage, result.Message);
        _noteRepositoryMock.Verify(repo => repo.GetByIdAsync(noteId), Times.Once);
    }

    [TestMethod]
    public async Task GetNoteByIdAsync_NoteExistsAndUserIsOwner_ReturnsNoteDto()
    {
        // Arrange
        Note note = CreateNote(id: 5, userId: UserId, text: "Buy milk", priority: Priority.High);

        _noteRepositoryMock
            .Setup(repo => repo.GetByIdAsync(5))
            .ReturnsAsync(note);

        // Act
        NoteDto result = await _noteService.GetNoteByIdAsync(5, UserId);

        // Assert
        Assert.AreEqual(5, result.Id);
        Assert.AreEqual("Buy milk", result.Text);
        Assert.AreEqual(Priority.High, result.Priority);
    }

    [TestMethod]
    public async Task GetNoteByIdAsync_WhenUserIsNotTheOwner_ThrowsNoteAccessDeniedException()
    {
        // Arrange
        Note note = CreateNote(id: 5, userId: UserId, text: "Buy milk");

        _noteRepositoryMock
            .Setup(repo => repo.GetByIdAsync(5))
            .ReturnsAsync(note);

        // Act & Assert
        NoteAccessDeniedException exception = await Assert.ThrowsExactlyAsync<NoteAccessDeniedException>(
            () => _noteService.GetNoteByIdAsync(5, 1000));
        Assert.AreEqual($"Note with id {note.Id} does not belong to you.", exception.Message);
    }

    #endregion


    #region Helper methods

    private static Note CreateNote(int id, int userId, string text, Priority priority = Priority.Low)
    {
        return new Note
        {
            Id = id,
            Text = text,
            Priority = priority,
            UserId = userId,
            User = CreateUser(userId),
            Tags = new List<Tag>()
        };
    }

    private static User CreateUser(int id)
    {
        return new User
        {
            Id = id,
            FirstName = "John",
            LastName = "Doe",
            Username = "johnny",
            Password = "hashed-password"
        };
    }

    #endregion

}
