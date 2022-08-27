namespace CustomerLibrary.BusinessEntities
{
    public class Note
    {
        public int NoteId { get; set; } = 0;
        public int CustomerId { get; set; } = 0;
        public string? Text { get; set; } = null;
    }
}
