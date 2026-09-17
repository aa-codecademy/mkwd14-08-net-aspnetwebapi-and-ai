using Moq;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;
using NotesApp.DTOs;
using NotesApp.Services.Implementation;
using NotesApp.Services.Interfaces;
using NotesApp.Domain.Enums;

namespace NotesApp.Tests
{
    [TestClass]
    public class NoteServiceUnitTests
    {
        private readonly INoteService _noteService;
        private readonly Mock<INoteRepository> _noteRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<ITagRepository> _tagRepository;

        public NoteServiceUnitTests()
        {
            _noteRepository = new Mock<INoteRepository>();
            _userRepository = new Mock<IUserRepository>();
            _tagRepository = new Mock<ITagRepository>();

            _noteService = new NoteService(_noteRepository.Object, _userRepository.Object, _tagRepository.Object);
        }

        [TestMethod]
        public void GetAllNotes_ShouldReturnNoteDtos()
        {
            //Arrange
            var notes = new List<Note>
            {
                new Note
                {
                    Id = 1,
                    Text = "Do your homework",
                    Priority = PriorityEnum.High,
                    UserId = 1,
                    User = new User
                    {
                        Id = 1,
                        FirstName = "Petko",
                        LastName = "Petkovski"
                    }
                },
                new Note
                {
                    Id = 2,
                    Text = "Go to the gym",
                    Priority = PriorityEnum.Medium,
                    UserId = 2,
                    User = new User
                    {
                        Id = 2,
                        FirstName = "Marko",
                        LastName = "Markovski"
                    }
                },
            };

            _noteRepository.Setup(x => x.GetAll()).Returns(notes);

            //Act
            var result = _noteService.GetAllNotes();

            //Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Do your homework", result[0].Text);
            Assert.AreEqual("Go to the gym", result[1].Text);
        }

        [TestMethod]
        public void GetAllNotes_ShouldReturnEmptyList_WhenNoNotesExist()
        {
            //Arrange
            _noteRepository.Setup(x => x.GetAll()).Returns(new List<Note>());

            //Act
            var result = _noteService.GetAllNotes();

            //Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetById_ShouldReturnNoteDto_WhenNoteExists()
        {
            //Arrange
            var note = new Note
            {
                Id = 1,
                Text = "Test note",
                Priority = PriorityEnum.Low,
                UserId = 1,
                User = new User { Id = 1, FirstName = "Test", LastName = "User" }
            };
            _noteRepository.Setup(x => x.GetById(1)).Returns(note);

            //Act
            var result = _noteService.GetById(1);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Test note", result.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void GetById_ShouldThrow_WhenNoteDoesNotExist()
        {
            //Arrange
            _noteRepository.Setup(x => x.GetById(1)).Returns((Note)null);

            //Act
            _noteService.GetById(1);
        }

        [TestMethod]
        public void AddNote_ShouldAddAndReturnNoteDto_WhenValid()
        {
            //Arrange
            var addNoteDto = new AddNoteDto
            {
                Text = "New note",
                Priority = PriorityEnum.High,
                UserId = 1,
                TagIds = new List<int> { 1, 2 }
            };
            var user = new User { Id = 1, FirstName = "Test", LastName = "User" };
            var tag1 = new Tag { Id = 1, Name = "Tag1" };
            var tag2 = new Tag { Id = 2, Name = "Tag2" };

            _userRepository.Setup(x => x.GetById(1)).Returns(user);
            _tagRepository.Setup(x => x.GetById(1)).Returns(tag1);
            _tagRepository.Setup(x => x.GetById(2)).Returns(tag2);

            //Act
            var result = _noteService.AddNote(addNoteDto);

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("New note", result.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNote_ShouldThrow_WhenNoteIsNull()
        {
            //Act
            _noteService.AddNote(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddNote_ShouldThrow_WhenTextIsNullOrEmpty()
        {
            //Arrange
            var addNoteDto = new AddNoteDto
            {
                Text = null,
                Priority = PriorityEnum.High,
                UserId = 1,
                TagIds = new List<int>()
            };

            //Act
            _noteService.AddNote(addNoteDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddNote_ShouldThrow_WhenUserDoesNotExist()
        {
            //Arrange
            var addNoteDto = new AddNoteDto
            {
                Text = "Test",
                Priority = PriorityEnum.High,
                UserId = 1,
                TagIds = new List<int>()
            };
            _userRepository.Setup(x => x.GetById(1)).Returns((User)null);

            //Act
            _noteService.AddNote(addNoteDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void AddNote_ShouldThrow_WhenTagDoesNotExist()
        {
            //Arrange
            var addNoteDto = new AddNoteDto
            {
                Text = "Test",
                Priority = PriorityEnum.High,
                UserId = 1,
                TagIds = new List<int> { 1 }
            };
            var user = new User { Id = 1, FirstName = "Test", LastName = "User" };
            _userRepository.Setup(x => x.GetById(1)).Returns(user);
            _tagRepository.Setup(x => x.GetById(1)).Returns((Tag)null);

            //Act
            _noteService.AddNote(addNoteDto);
        }

        [TestMethod]
        public void DeleteById_ShouldCallRepositoryDelete()
        {
            //Arrange
            _noteRepository.Setup(x => x.Delete(1));

            //Act
            _noteService.DeleteById(1);

            //Assert
            _noteRepository.Verify(x => x.Delete(1), Times.Once);
        }

        [TestMethod]
        public void UpdateNote_ShouldUpdateNote_WhenValid()
        {
            //Arrange
            var updateNoteDto = new UpdateNoteDto
            {
                Id = 1,
                Text = "Updated text",
                Priority = PriorityEnum.Medium,
                UserId = 1,
                TagIds = new List<int> { 1 }
            };
            var note = new Note
            {
                Id = 1,
                Text = "Old text",
                Priority = PriorityEnum.Low,
                UserId = 1,
                Tags = new List<Tag>(),
                User = new User { Id = 1, FirstName = "Test", LastName = "User" }
            };
            var tag = new Tag { Id = 1, Name = "Tag1" };

            _noteRepository.Setup(x => x.GetById(1)).Returns(note);
            _tagRepository.Setup(x => x.GetById(1)).Returns(tag);

            //Act
            _noteService.UpdateNote(updateNoteDto);

            //Assert
            _noteRepository.Verify(x => x.Update(It.Is<Note>(n => n.Text == "Updated text" && n.Priority == PriorityEnum.Medium && n.Tags.Contains(tag))), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateNote_ShouldThrow_WhenNoteIsNull()
        {
            //Act
            _noteService.UpdateNote(null);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNote_ShouldThrow_WhenNoteDoesNotExist()
        {
            //Arrange
            var updateNoteDto = new UpdateNoteDto
            {
                Id = 1,
                Text = "Updated text",
                Priority = PriorityEnum.Medium,
                UserId = 1,
                TagIds = new List<int>()
            };
            _noteRepository.Setup(x => x.GetById(1)).Returns((Note)null);

            //Act
            _noteService.UpdateNote(updateNoteDto);
        }

        [TestMethod]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void UpdateNote_ShouldThrow_WhenUserIsNotOwner()
        {
            //Arrange
            var updateNoteDto = new UpdateNoteDto
            {
                Id = 1,
                Text = "Updated text",
                Priority = PriorityEnum.Medium,
                UserId = 2,
                TagIds = new List<int>()
            };
            var note = new Note
            {
                Id = 1,
                Text = "Old text",
                Priority = PriorityEnum.Low,
                UserId = 1
            };
            _noteRepository.Setup(x => x.GetById(1)).Returns(note);

            //Act
            _noteService.UpdateNote(updateNoteDto);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateNote_ShouldThrow_WhenTextIsNullOrEmpty()
        {
            //Arrange
            var updateNoteDto = new UpdateNoteDto
            {
                Id = 1,
                Text = null,
                Priority = PriorityEnum.Medium,
                UserId = 1,
                TagIds = new List<int>()
            };
            var note = new Note
            {
                Id = 1,
                Text = "Old text",
                Priority = PriorityEnum.Low,
                UserId = 1
            };
            _noteRepository.Setup(x => x.GetById(1)).Returns(note);

            //Act
            _noteService.UpdateNote(updateNoteDto);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void UpdateNote_ShouldThrow_WhenTagDoesNotExist()
        {
            //Arrange
            var updateNoteDto = new UpdateNoteDto
            {
                Id = 1,
                Text = "Updated text",
                Priority = PriorityEnum.Medium,
                UserId = 1,
                TagIds = new List<int> { 1 }
            };
            var note = new Note
            {
                Id = 1,
                Text = "Old text",
                Priority = PriorityEnum.Low,
                UserId = 1
            };
            _noteRepository.Setup(x => x.GetById(1)).Returns(note);
            _tagRepository.Setup(x => x.GetById(1)).Returns((Tag)null);

            //Act
            _noteService.UpdateNote(updateNoteDto);
        }
    }
}
