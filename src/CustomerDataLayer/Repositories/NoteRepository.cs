using CustomerLibrary.BusinessEntities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CustomerLibrary.Repositories
{
    public class NoteRepository : BaseRepository, IRepository<Note>
    {
        private static SqlParameter[] GetDefaultParameters(Note note)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = note.CustomerId },
                new SqlParameter("@Text", System.Data.SqlDbType.NVarChar, 100) { Value = note.Text }
            };
        }

        private static Note GetNote(SqlDataReader reader)
        {
            return new Note
            {
                NoteId = (int)reader["NoteID"],
                CustomerId = (int)reader["CustomerId"],
                Text = (string)reader["Text"]
            };
        }

        public int? Create(Note entity)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "INSERT INTO [Note] ([CustomerId], [Text]) " +
                    "OUTPUT INSERTED.[NoteId] " +
                    "VALUES (@CustomerId, @Text)",
                    connection);
                command.Parameters.AddRange(GetDefaultParameters(entity));

                var reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
                int noteId = (int)reader["NoteId"];

                return noteId;
            }
        }

        public Note Read(int entityId)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "SELECT * FROM [Note] " +
                    "WHERE [NoteId] = @NoteId",
                    connection);
                command.Parameters.Add(
                    new SqlParameter("@NoteId", System.Data.SqlDbType.Int) { Value = entityId });

                var reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
                var note = GetNote(reader);

                return note;
            }
        }

        public bool Update(Note entity)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "UPDATE [Note] " +
                    "SET [CustomerId] = @CustomerId, [Text] = @Text " +
                    "WHERE [NoteId] = @NoteId",
                    connection);
                command.Parameters.Add(
                    new SqlParameter("@NoteId", System.Data.SqlDbType.Int) { Value = entity.NoteId });
                command.Parameters.AddRange(GetDefaultParameters(entity));

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
        }

        public bool Delete(int entityId)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "DELETE FROM [Note] " +
                    "WHERE [NoteId] = @NoteId",
                    connection);
                command.Parameters.Add(
                    new SqlParameter("@NoteId", System.Data.SqlDbType.Int) { Value = entityId });

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
        }

        public int Count()
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "SELECT COUNT(*) FROM [Note]",
                    connection);

                int count = (int)command.ExecuteScalar();

                return count;
            }
        }

        public List<Note> Read(int offset, int count)
        {
            if (count <= 0)
            {
                return new List<Note>();
            }

            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "SELECT * FROM [Note] " +
                    "ORDER BY [NoteId] " +
                    "OFFSET @Offset ROWS " +
                    "FETCH NEXT @Count ROWS ONLY",
                    connection);
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@Offset", System.Data.SqlDbType.Int) { Value = offset },
                    new SqlParameter("@Count", System.Data.SqlDbType.Int) { Value = count }
                });

                var reader = command.ExecuteReader();

                var notes = new List<Note>();
                while (reader.Read())
                {
                    notes.Add(GetNote(reader));
                }

                return notes;
            }
        }

        public void DeleteAll()
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "DELETE FROM [Note]",
                    connection);

                command.ExecuteNonQuery();
            }
        }

        public int DeleteByCustomerId(int customerId)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "DELETE FROM [Note] " +
                    "WHERE [CustomerId] = @CustomerId",
                    connection);
                command.Parameters.Add(
                    new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = customerId });

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows;
            }
        }
    }
}
