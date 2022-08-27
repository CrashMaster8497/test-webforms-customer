using CustomerLibrary.BusinessEntities;
using System.Data.SqlClient;

namespace CustomerLibrary.Repositories
{
    public class NoteRepository : BaseRepository, IRepository<Note>
    {
        public int? Create(Note entity)
        {
            var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "INSERT INTO [Note] ([CustomerID], [Text]) " +
                "OUTPUT INSERTED.[NoteID] " +
                "VALUES (@CustomerId, @Text)",
                connection);
            command.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = entity.CustomerId },
                new SqlParameter("@Text", System.Data.SqlDbType.NVarChar) { Value = entity.Text }
            });

            var sqlReader = command.ExecuteReader();

            if (!sqlReader.HasRows)
            {
                return null;
            }

            sqlReader.Read();
            int noteId = (int)sqlReader["NoteID"];

            return noteId;
        }

        public Note? Read(int entityId)
        {
            var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "SELECT * FROM [Note] " +
                "WHERE [NoteID] = @NoteId",
                connection);
            command.Parameters.Add(
                new SqlParameter("@NoteId", System.Data.SqlDbType.Int) { Value = entityId });

            var sqlReader = command.ExecuteReader();

            if (!sqlReader.HasRows)
            {
                return null;
            }

            sqlReader.Read();
            var note = new Note
            {
                NoteId = (int)sqlReader["NoteID"],
                CustomerId = (int)sqlReader["CustomerID"],
                Text = (string)sqlReader["Text"]
            };

            return note;
        }

        public bool Update(Note entity)
        {
            var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "UPDATE [Note] " +
                "SET [CustomerID] = @CustomerId, [Text] = @Text " +
                "WHERE [NoteID] = @NoteId",
                connection);
            command.Parameters.AddRange(new SqlParameter[]
            {
                new SqlParameter("@NoteId", System.Data.SqlDbType.Int) { Value = entity.NoteId },
                new SqlParameter("@CustomerID", System.Data.SqlDbType.Int) { Value = entity.CustomerId },
                new SqlParameter("@Text", System.Data.SqlDbType.NVarChar) { Value = entity.Text }
            });

            int affectedRows = command.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public bool Delete(int entityId)
        {
            var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "DELETE FROM [Note] " +
                "WHERE [NoteID] = @NoteId",
                connection);
            command.Parameters.Add(
                new SqlParameter("@NoteId", System.Data.SqlDbType.Int) { Value = entityId });

            int affectedRows = command.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public void DeleteAll()
        {
            var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "DELETE FROM [Note]",
                connection);

            command.ExecuteNonQuery();
        }
    }
}
