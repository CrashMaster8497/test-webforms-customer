using CustomerLibrary.BusinessEntities;
using System.Data.SqlClient;

namespace CustomerLibrary.Repositories
{
    public class AddressRepository : BaseRepository, IRepository<Address>
    {
        public int? Create(Address entity)
        {
            using var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "INSERT INTO [Address] ([CustomerID], [AddressLine], [AddressLine2], [AddressType], [City], [PostalCode], [State], [Country]) " +
                "OUTPUT INSERTED.[AddressID] " +
                "VALUES (@CustomerId, @AddressLine, @AddressLine2, @AddressType, @City, @PostalCode, @State, @Country)",
                connection);
            command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = entity.CustomerId },
                    new SqlParameter("@AddressLine", System.Data.SqlDbType.NVarChar) { Value = entity.AddressLine },
                    new SqlParameter("@AddressLine2", System.Data.SqlDbType.NVarChar) { Value = entity.AddressLine2 },
                    new SqlParameter("@AddressType", System.Data.SqlDbType.VarChar) { Value = entity.AddressType },
                    new SqlParameter("@City", System.Data.SqlDbType.VarChar) { Value = entity.City },
                    new SqlParameter("@PostalCode", System.Data.SqlDbType.VarChar) { Value = entity.PostalCode },
                    new SqlParameter("@State", System.Data.SqlDbType.VarChar) { Value = entity.State },
                    new SqlParameter("@Country", System.Data.SqlDbType.VarChar) { Value = entity.Country }
                });

            var sqlReader = command.ExecuteReader();

            if (!sqlReader.HasRows)
            {
                return null;
            }

            sqlReader.Read();
            int addressId = (int)sqlReader["AddressID"];

            return addressId;
        }

        public Address? Read(int entityId)
        {
            var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "SELECT * FROM [Address] " +
                "WHERE [AddressID] = @AddressId",
                connection);
            command.Parameters.Add(
                new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = entityId });

            var sqlReader = command.ExecuteReader();

            if (!sqlReader.HasRows)
            {
                return null;
            }

            sqlReader.Read();
            var address = new Address()
            {
                AddressId = (int)sqlReader["AddressID"],
                CustomerId = (int)sqlReader["CustomerID"],
                AddressLine = (string)sqlReader["AddressLine"],
                AddressLine2 = (string)sqlReader["AddressLine2"],
                AddressType = Enum.Parse<AddressType>((string)sqlReader["AddressType"], true),
                City = (string)sqlReader["City"],
                PostalCode = (string)sqlReader["PostalCode"],
                State = (string)sqlReader["State"],
                Country = (string)sqlReader["Country"],
            };

            return address;
        }

        public bool Update(Address entity)
        {
            var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "UPDATE [Address] " +
                "SET [CustomerID] = @CustomerId, [AddressLine] = @AddressLine, [AddressLine2] = @AddressLine2, [AddressType] = @AddressType, " +
                "    [City] = @City, [PostalCode] = @PostalCode, [State] = @State, [Country] = @Country " +
                "WHERE [AddressID] = @AddressId",
                connection);
            command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = entity.AddressId },
                    new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = entity.CustomerId },
                    new SqlParameter("@AddressLine", System.Data.SqlDbType.NVarChar) { Value = entity.AddressLine },
                    new SqlParameter("@AddressLine2", System.Data.SqlDbType.NVarChar) { Value = entity.AddressLine2 },
                    new SqlParameter("@AddressType", System.Data.SqlDbType.VarChar) { Value = entity.AddressType },
                    new SqlParameter("@City", System.Data.SqlDbType.VarChar) { Value = entity.City },
                    new SqlParameter("@PostalCode", System.Data.SqlDbType.VarChar) { Value = entity.PostalCode },
                    new SqlParameter("@State", System.Data.SqlDbType.VarChar) { Value = entity.State },
                    new SqlParameter("@Country", System.Data.SqlDbType.VarChar) { Value = entity.Country }
                });

            int affectedRows = command.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public bool Delete(int entityId)
        {
            using var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "DELETE FROM [Address] " +
                "WHERE [AddressID] = @AddressId",
                connection);
            command.Parameters.Add(
                new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = entityId });

            int affectedRows = command.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public void DeleteAll()
        {
            using var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "DELETE FROM [Address]",
                connection);

            command.ExecuteNonQuery();
        }
    }
}
