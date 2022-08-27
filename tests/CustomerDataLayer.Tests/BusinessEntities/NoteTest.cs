using CustomerLibrary.BusinessEntities;

namespace CustomerLibrary.Tests.BusinessEntities
{
    public class NoteTest
    {
        [Fact]
        public void ShouldBeAbleToCreateNote()
        {
            var note = new Note();

            Assert.NotNull(note);
            Assert.Equal(0, note.NoteId);
            Assert.Equal(0, note.CustomerId);
            Assert.Null(note.Text);
        }
    }
}
