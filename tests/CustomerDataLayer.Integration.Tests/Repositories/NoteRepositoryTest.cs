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

        [Theory]
        [MemberData(nameof(GenerateNotes))]
        public void ShouldBeAbleToCreateAndReadNote(Note note)
        {
            NoteRepositoryFixture.DeleteAllNotes();

            int noteId = NoteRepositoryFixture.CreateNote(note).Value;
            note.NoteId = noteId;

            var readNote = NoteRepositoryFixture.ReadNote(noteId);

            readNote.Should().NotBeNull();
            readNote.Should().BeEquivalentTo(note);
        }

        [Fact]
        public void ShouldNotReadByWrongId()
        {
            NoteRepositoryFixture.DeleteAllNotes();

            var note = NoteRepositoryFixture.GetMinNote();
            NoteRepositoryFixture.CreateNote(note);

            var readNote = NoteRepositoryFixture.ReadNote(0);

            readNote.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(GenerateNotes))]
        public void ShouldBeAbleTOUpdateNote(Note note)
        {
            NoteRepositoryFixture.DeleteAllNotes();

            int noteId = NoteRepositoryFixture.CreateNote(note).Value;

            var modifiedNote = NoteRepositoryFixture.ReadNote(noteId);
            modifiedNote.Text = "new text";
            bool isUpdated = NoteRepositoryFixture.UpdateNote(modifiedNote);
            var updatedNote = NoteRepositoryFixture.ReadNote(noteId);

            isUpdated.Should().BeTrue();
            updatedNote.Should().BeEquivalentTo(modifiedNote);
        }

        [Fact]
        public void ShouldNotUpdateByWrongId()
        {
            NoteRepositoryFixture.DeleteAllNotes();

            var note = NoteRepositoryFixture.GetMinNote();
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

            var note = NoteRepositoryFixture.GetMinNote();
            int noteId = NoteRepositoryFixture.CreateNote(note).Value;

            bool isDeleted = NoteRepositoryFixture.DeleteNote(noteId);
            var deletedNote = NoteRepositoryFixture.ReadNote(noteId);

            isDeleted.Should().BeTrue();
            deletedNote.Should().BeNull();
        }

        [Fact]
        public void ShouldNotDeleteByWrongId()
        {
            NoteRepositoryFixture.DeleteAllNotes();

            var note = NoteRepositoryFixture.GetMinNote();
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
        [MemberData(nameof(GenerateDataForReadByOffsetCount))]
        public void ShouldBeAbleToReadByOffsetAndCount(List<Note> notes, int offset, int count)
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

        [Theory]
        [MemberData(nameof(GenerateDataForReadByCustomerId))]
        public void ShouldBeAbleToReadByCustomerId(List<List<Note>> notes, List<Customer> customers, List<int> offsets, List<int> counts)
        {
            NoteRepositoryFixture.DeleteAllNotes();

            for (int i = 0; i < customers.Count; i++)
            {
                int customerId = CustomerRepositoryFixture.CreateCustomer(customers[i]).Value;
                customers[i].CustomerId = customerId;

                foreach (var note in notes[i])
                {
                    int noteId = NoteRepositoryFixture.CreateNote(note, customers[i]).Value;
                    note.NoteId = noteId;
                }
            }

            for (int i = 0; i < customers.Count; i++)
            {
                var readNotes = NoteRepositoryFixture.ReadNotesByCustomerId(customers[i].CustomerId, offsets[i], counts[i]);

                readNotes.Should().BeEquivalentTo(notes[i].Skip(offsets[i]).Take(counts[i]));
            }
        }

        [Theory]
        [MemberData(nameof(GenerateDataForDeleteByCustomerId))]
        public void ShouldBeAbleToDeleteByCustomerId(List<List<Note>> notes, List<Customer> customers)
        {
            NoteRepositoryFixture.DeleteAllNotes();

            for (int i = 0; i < customers.Count; i++)
            {
                int customerId = CustomerRepositoryFixture.CreateCustomer(customers[i]).Value;
                customers[i].CustomerId = customerId;

                foreach (var note in notes[i])
                {
                    NoteRepositoryFixture.CreateNote(note, customers[i]);
                }
            }

            for (int i = 0; i < customers.Count; i++)
            {
                int deletedRows = NoteRepositoryFixture.DeleteNotesByCustomerId(customers[i].CustomerId);
                var deletedNotes = NoteRepositoryFixture.ReadNotesByCustomerId(customers[i].CustomerId, 0, notes[i].Count);

                deletedRows.Should().Be(notes[i].Count);
                deletedNotes.Should().BeEmpty();

                for (int j = i + 1; j < customers.Count; i++)
                {
                    var notDeletedNotes = NoteRepositoryFixture.ReadNotesByCustomerId(customers[i].CustomerId, 0, notes[i].Count);

                    notDeletedNotes.Should().NotBeEmpty();
                }
            }
        }

        private static IEnumerable<object[]> GenerateNotes()
        {
            yield return new object[] { NoteRepositoryFixture.GetMinNote() };
            yield return new object[] { NoteRepositoryFixture.GetMaxNote() };
        }

        private static IEnumerable<object[]> GenerateDataForCount()
        {
            yield return new object[] { new List<Note>(0).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList() };
            yield return new object[] { new List<Note>(1).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList() };
            yield return new object[] { new List<Note>(10).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList() };
        }

        private static IEnumerable<object[]> GenerateDataForReadByOffsetCount()
        {
            yield return new object[]
            {
                new List<Note>(6).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                2,
                2
            };
            yield return new object[]
            {
                new List<Note>(6).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                5,
                3
            };
            yield return new object[]
            {
                new List<Note>(6).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                6,
                2
            };
            yield return new object[]
            {
                new List<Note>(6).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                2,
                0
            };
            yield return new object[]
            {
                new List<Note>(6).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                7,
                1
            };
        }

        private static IEnumerable<object[]> GenerateDataForDeleteAll()
        {
            yield return new object[] { new List<Note>(0).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList() };
            yield return new object[] { new List<Note>(1).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList() };
            yield return new object[] { new List<Note>(10).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList() };
        }

        private static IEnumerable<object[]> GenerateDataForReadByCustomerId()
        {
            yield return new object[]
            {
                new List<List<Note>>
                {
                    new List<Note>(1).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                    new List<Note>(0).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                    new List<Note>(4).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList()
                },
                new List<Customer>(3).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList(),
                new List<int> { 0, 0, 0 },
                new List<int> { 1, 0, 4 }
            };
            yield return new object[]
            {
                new List<List<Note>>
                {
                    new List<Note>(1).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                    new List<Note>(0).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                    new List<Note>(4).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList()
                },
                new List<Customer>(3).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList(),
                new List<int> { 1, 1, 2 },
                new List<int> { 0, 2, 4 }
            };
        }

        private static IEnumerable<object[]> GenerateDataForDeleteByCustomerId()
        {
            yield return new object[]
            {
                new List<List<Note>>
                {
                    new List<Note>(1).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                    new List<Note>(0).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList(),
                    new List<Note>(3).Select(note => note = NoteRepositoryFixture.GetMinNote()).ToList()
                },
                new List<Customer>(3).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList()
            };
        }
    }

    public static class NoteRepositoryFixture
    {
        public static Note GetMinNote()
        {
            return new Note
            {
                Text = "text"
            };
        }

        public static Note GetMaxNote()
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
            var customer = CustomerRepositoryFixture.GetMinCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
            note.CustomerId = customerId;

            var noteRepository = new NoteRepository();
            return noteRepository.Create(note);
        }

        public static int? CreateNote(Note note, Customer customer)
        {
            var noteRepository = new NoteRepository();
            note.CustomerId = customer.CustomerId;
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

        public static List<Note> ReadNotesByCustomerId(int customerId, int offset, int count)
        {
            var noteRepository = new NoteRepository();
            return noteRepository.ReadByCustomerId(customerId, offset, count);
        }

        public static int DeleteNotesByCustomerId(int customerId)
        {
            var noteRepository = new NoteRepository();
            return noteRepository.DeleteByCustomerId(customerId);
        }
    }
}
