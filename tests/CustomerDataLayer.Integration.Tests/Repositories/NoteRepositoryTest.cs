using CustomerLibrary.BusinessEntities;
using CustomerLibrary.Repositories;
using FluentAssertions;

namespace CustomerLibrary.Integration.Tests.Repositories
{
    [Collection("CustomerLibraryTests")]
    public class NoteRepositoryTest
    {
        [Fact]
        public void ShouldBeAbleToCreateNoteRepository()
        {
            var noteRepository = NoteRepositoryFixture.GetNoteRepository();

            noteRepository.Should().NotBeNull();
        }

        [Fact]
        public void ShouldBeAbleToCreateAndReadNote()
        {
            NoteRepositoryFixture.DeleteAllNotes();

            var note = NoteRepositoryFixture.GetDefaultNote();
            int noteId = NoteRepositoryFixture.CreateNote(note).Value;
            note.NoteId = noteId;

            var readNote = NoteRepositoryFixture.ReadNote(noteId);

            readNote.Should().NotBeNull();
            readNote.Should().BeEquivalentTo(note);
        }

        [Fact]
        public void ShouldNotReadWithWrongId()
        {
            NoteRepositoryFixture.DeleteAllNotes();

            var note = NoteRepositoryFixture.GetDefaultNote();
            NoteRepositoryFixture.CreateNote(note);

            var readNote = NoteRepositoryFixture.ReadNote(0);

            readNote.Should().BeNull();
        }

        [Fact]
        public void ShouldBeAbleTOUpdateNote()
        {
            NoteRepositoryFixture.DeleteAllNotes();

            var note = NoteRepositoryFixture.GetDefaultNote();
            int noteId = NoteRepositoryFixture.CreateNote(note).Value;

            var modifiedNote = NoteRepositoryFixture.ReadNote(noteId);
            modifiedNote.Text = "new text";
            bool isUpdated = NoteRepositoryFixture.UpdateNote(modifiedNote);
            var updatedNote = NoteRepositoryFixture.ReadNote(noteId);

            isUpdated.Should().BeTrue();
            updatedNote.Should().BeEquivalentTo(modifiedNote);
        }

        [Fact]
        public void ShouldNotUpdateWithWrongId()
        {
            NoteRepositoryFixture.DeleteAllNotes();

            var note = NoteRepositoryFixture.GetDefaultNote();
            int noteId = NoteRepositoryFixture.CreateNote(note).Value;
            var createdNote = NoteRepositoryFixture.ReadNote(noteId);

            var modifiedNote = NoteRepositoryFixture.ReadNote(noteId);
            modifiedNote.NoteId = 0;
            modifiedNote.Text = "new text";
            bool isUpdated = NoteRepositoryFixture.UpdateNote(modifiedNote);
            var updatedNote = NoteRepositoryFixture.ReadNote(noteId);

            isUpdated.Should().BeFalse();
            updatedNote.Should().BeEquivalentTo(createdNote);
        }

        [Fact]
        public void ShouldBeAbleToDeleteNote()
        {
            NoteRepositoryFixture.DeleteAllNotes();

            var note = NoteRepositoryFixture.GetDefaultNote();
            int noteId = NoteRepositoryFixture.CreateNote(note).Value;

            bool isDeleted = NoteRepositoryFixture.DeleteNote(noteId);
            var deletedNote = NoteRepositoryFixture.ReadNote(noteId);

            isDeleted.Should().BeTrue();
            deletedNote.Should().BeNull();
        }

        [Fact]
        public void ShouldNotDeleteWithWrongId()
        {
            NoteRepositoryFixture.DeleteAllNotes();

            var note = NoteRepositoryFixture.GetDefaultNote();
            int noteId = NoteRepositoryFixture.CreateNote(note).Value;
            var createdNote = NoteRepositoryFixture.ReadNote(noteId);

            bool isDeleted = NoteRepositoryFixture.DeleteNote(0);
            var deletedNote = NoteRepositoryFixture.ReadNote(noteId);

            isDeleted.Should().BeFalse();
            deletedNote.Should().BeEquivalentTo(createdNote);
        }

        [Theory]
        [MemberData(nameof(GenerateDataForCount))]
        public void ShouldBeAbleToCountNotes(List<Note> notes)
        {
            NoteRepositoryFixture.DeleteAllNotes();

            foreach (var note in notes)
            {
                NoteRepositoryFixture.CreateNote(note);
            }

            var notesCount = NoteRepositoryFixture.CountNotes();

            notesCount.Should().Be(notes.Count);
        }

        [Theory]
        [MemberData(nameof(GenerateDataForReadWithOffsetCount))]
        public void ShouldBeAbleToReadWithOffsetAndCount(List<Note> notes, int offset, int count)
        {
            NoteRepositoryFixture.DeleteAllNotes();

            foreach (var note in notes)
            {
                int noteId = NoteRepositoryFixture.CreateNote(note).Value;
                note.NoteId = noteId;
            }

            var readNotes = NoteRepositoryFixture.ReadNotes(offset, count);

            readNotes.Should().NotBeNull();
            readNotes.Should().BeEquivalentTo(notes.Skip(offset).Take(count));
        }

        [Theory]
        [MemberData(nameof(GenerateDataForDeleteAll))]
        public void ShouldBeAbleToDeleteAllNotes(List<Note> notes)
        {
            NoteRepositoryFixture.DeleteAllNotes();

            foreach (var note in notes)
            {
                NoteRepositoryFixture.CreateNote(note);
            }

            NoteRepositoryFixture.DeleteAllNotes();

            var deletedNotes = NoteRepositoryFixture.ReadNotes(0, notes.Count);

            deletedNotes.Should().NotBeNull();
            deletedNotes.Should().BeEmpty();
        }

        private static IEnumerable<object[]> GenerateDataForCount()
        {
            List<Note> notes;

            notes = new List<Note>(0);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[] { notes };

            notes = new List<Note>(1);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[] { notes };

            notes = new List<Note>(10);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[] { notes };
        }

        private static IEnumerable<object[]> GenerateDataForReadWithOffsetCount()
        {
            List<Note> notes;

            notes = new List<Note>(6);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[]
            {
                notes,
                2,
                2
            };

            notes = new List<Note>(6);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[]
            {
                notes,
                5,
                3
            };

            notes = new List<Note>(6);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[]
            {
                notes,
                6,
                2
            };

            notes = new List<Note>(6);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[]
            {
                notes,
                2,
                0
            };

            notes = new List<Note>(6);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[]
            {
                notes,
                7,
                1
            };
        }

        private static IEnumerable<object[]> GenerateDataForDeleteAll()
        {
            List<Note> notes;

            notes = new List<Note>(0);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[] { notes };

            notes = new List<Note>(1);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[] { notes };

            notes = new List<Note>(10);
            notes.ForEach(note => note = NoteRepositoryFixture.GetDefaultNote());
            yield return new object[] { notes };
        }
    }

    public static class NoteRepositoryFixture
    {
        public static Note GetDefaultNote()
        {
            return new Note
            {
                Text = "text"
            };
        }

        public static NoteRepository GetNoteRepository()
        {
            return new NoteRepository();
        }

        public static int? CreateNote(Note note)
        {
            var customer = CustomerRepositoryFixture.GetDefaultCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
            note.CustomerId = customerId;

            var noteRepository = new NoteRepository();
            return noteRepository.Create(note);
        }

        public static Note? ReadNote(int noteId)
        {
            var noteRepository = new NoteRepository();
            return noteRepository.Read(noteId);
        }

        public static bool UpdateNote(Note note)
        {
            var noteRepository = new NoteRepository();
            return noteRepository.Update(note);
        }

        public static bool DeleteNote(int noteId)
        {
            var noteRepository = new NoteRepository();
            return noteRepository.Delete(noteId);
        }

        public static int CountNotes()
        {
            var noteRepository = new NoteRepository();
            return noteRepository.Count();
        }

        public static List<Note> ReadNotes(int offset, int count)
        {
            var noteRepository = new NoteRepository();
            return noteRepository.Read(offset, count);
        }

        public static void DeleteAllNotes()
        {
            var noteRepository = new NoteRepository();
            noteRepository.DeleteAll();
        }
    }
}
