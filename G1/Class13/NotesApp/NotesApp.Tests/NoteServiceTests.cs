using Moq;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Enums;
using NotesApp.Domain.Models;
using NotesApp.Dtos;
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

    private NoteService _noteService;

    private const int UserId = 100;

    [TestInitialize]
    public void Setup()
    {
        _noteRepositoryMock = new Mock<INoteRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();

        _noteService = new NoteService(
            _noteRepositoryMock.Object,
            _userRepositoryMock.Object,
            _tagRepositoryMock.Object
        );
    }

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
