using Microsoft.Identity.Client;
using NotesApp.DataAccess.Interfaces;
using NotesApp.Domain.Models;
using NotesApp.DTOs;
using NotesApp.Mappers;
using NotesApp.Services.Interfaces;

namespace NotesApp.Services.Implementation
{
    public class NoteService : INoteService
    {
        //here we use the interface. In Program.cs we registered the dependency (what implementation of this interface should be called)
        private readonly INoteRepository _noteRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITagRepository _tagRepository;

        public NoteService(INoteRepository noteRepository, IUserRepository userRepository, ITagRepository tagRepository)
        {
            _noteRepository = noteRepository;
            _userRepository = userRepository;
            _tagRepository = tagRepository;
        }

        public NoteDto AddNote(AddNoteDto note)
        {

            if (note == null)
            {
                throw new ArgumentNullException("Note cannot be null");
            }

            if (string.IsNullOrEmpty(note.Text))
            {

                throw new ArgumentNullException("Text is a required field");
            }

            //add validations for all properties

            User user = _userRepository.GetById(note.UserId);
            if (user == null)
            {
                throw new NullReferenceException($"User with id {note.UserId} does not exist!");
            }

            List<Tag> tags = new List<Tag>();

            foreach (var tagId in note.TagIds)
            {
                var tag = _tagRepository.GetById(tagId);
                if (tag == null)
                {
                    throw new NullReferenceException($"Tag with id {tagId} does not exist!");
                }

                tags.Add(tag);
            }

            Note newNote = note.ToNote();
            newNote.User = user; //the properties that need to be fetched from DB should be mapped directly in the service. We SHOULD NOT have calls to repo in the mapper. The mapper is only a helper class for mapping
            newNote.Tags = tags;

            _noteRepository.Add(newNote);

            //if we want to return our new note to the controller in order to show the details for the note, we return DTO object

            return newNote.ToNoteDto(); //again, we reuse the mapper
        }

        public void DeleteById(int id)
        {
            _noteRepository.Delete(id);
        }

        public List<NoteDto> GetAllNotes()
        {
            //1. get all notes from db - the repo returns to the service domain models
            List<Note> notesDb = _noteRepository.GetAll();

            //2. Map the notes into dto - we MUST NOT return domain models to the controller

            List<NoteDto> notes = notesDb.Select(x => x.ToNoteDto()).ToList();

            return notes;
        }

        public NoteDto GetById(int id)
        {
            //1. get the note from db
            var note = _noteRepository.GetById(id);

            if (note == null)
            {
                throw new NullReferenceException($"Note with id {id} does not exist");
            }

            return note.ToNoteDto(); //we reuse the mapper for note
        }

        public void UpdateNote(UpdateNoteDto note)
        {
            //validation
            if (note == null)
            {
                throw new ArgumentNullException("Note cannot be null");
            }

            //check if note with this id exists - check if there is a note with this id that we can update
            Note noteDb = _noteRepository.GetById(note.Id);
            if (noteDb == null)
            {
                throw new NullReferenceException($"Note with id {note.Id} was not found");
            }

            var isValidUser = noteDb.UserId == note.UserId;
            if (!isValidUser)
            {
                throw new UnauthorizedAccessException($"This note is not a note of the user with id {note.UserId}");
            }

            if (string.IsNullOrEmpty(note.Text))
            {
                throw new ArgumentNullException("The text is a required field");
            }

            List<Tag> tags = new List<Tag>();

            foreach (var tagId in note.TagIds)
            {
                var tag = _tagRepository.GetById(tagId);
                if (tag == null)
                {
                    throw new NullReferenceException($"Tag with id {tagId} was not found");
                }
                tags.Add(tag);
            }

            //update
            noteDb.Text = note.Text;
            noteDb.Priority = note.Priority;
            noteDb.Tags = tags;

            //save to db
            _noteRepository.Update(noteDb); //we need to call the repo in order to save the changes to the db
        }
    }
}
