using CustomerLibrary.BusinessEntities;
using CustomerLibrary.Repositories;
using System;

namespace CustomerDataLayer.WebForms
{
    public partial class NoteEdit : System.Web.UI.Page
    {
        private readonly NoteRepository _noteRepository;
        protected Note Note;

        public NoteEdit()
        {
            _noteRepository = new NoteRepository();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string noteIdString = Request.QueryString["id"];
            int.TryParse(noteIdString, out int noteId);

            Note = _noteRepository.Read(noteId);

            if (Note == null)
            {
                Response.Redirect("CustomerList.aspx");
            }

            if (IsPostBack)
            {
                return;
            }

            this.noteId.Text = Note.NoteId.ToString();
            customerId.Text = Note.CustomerId.ToString();
            text.Text = Note.Text;
        }

        protected void OnClickSave(object sender, EventArgs e)
        {
            var modifiedNote = new Note
            {
                NoteId = Note.NoteId,
                CustomerId = Note.CustomerId,
                Text = text.Text
            };
            _noteRepository.Update(modifiedNote);

            Response.Redirect($"CustomerEdit.aspx?id={Note.CustomerId}");
        }

        protected void OnClickDelete(object sender, EventArgs e)
        {
            _noteRepository.Delete(Note.NoteId);

            Response.Redirect($"CustomerEdit.aspx?id={Note.CustomerId}");
        }
    }
}
