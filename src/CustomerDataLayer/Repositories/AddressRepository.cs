using CustomerLibrary.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CustomerLibrary.Repositories
{
    public class AddressRepository : BaseRepository, IRepository<Address>
    {
        private static SqlParameter[] GetDefaultParameters(Address address)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = address.CustomerId },
                new SqlParameter("@AddressLine", System.Data.SqlDbType.NVarChar, 100) { Value = address.AddressLine },
                new SqlParameter("@AddressLine2", System.Data.SqlDbType.NVarChar, 100) { Value = address.AddressLine2 },
                new SqlParameter("@AddressType", System.Data.SqlDbType.VarChar, 8) { Value = address.AddressType },
                new SqlParameter("@City", System.Data.SqlDbType.VarChar, 50) { Value = address.City },
                new SqlParameter("@PostalCode", System.Data.SqlDbType.VarChar, 6) { Value = address.PostalCode },
                new SqlParameter("@State", System.Data.SqlDbType.VarChar, 20) { Value = address.State },
                new SqlParameter("@Country", System.Data.SqlDbType.VarChar, 50) { Value = address.Country }
            };
        }

        private static Address GetAddress(SqlDataReader reader)
        {
            Enum.TryParse((string)reader["AddressType"], true, out AddressType addressType);
            return new Address
            {
                AddressId = (int)reader["AddressID"],
                CustomerId = (int)reader["CustomerID"],
                AddressLine = (string)reader["AddressLine"],
                AddressLine2 = (string)reader["AddressLine2"],
                AddressType = addressType,
                City = (string)reader["City"],
                PostalCode = (string)reader["PostalCode"],
                State = (string)reader["State"],
                Country = (string)reader["Country"],
            };
        }

        public int? Create(Address entity)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "INSERT INTO [Address] ([CustomerId], [AddressLine], [AddressLine2], [AddressType], [City], [PostalCode], [State], [Country]) " +
                    "OUTPUT INSERTED.[AddressId] " +
                    "VALUES (@CustomerId, @AddressLine, @AddressLine2, @AddressType, @City, @PostalCode, @State, @Country)",
                    connection);
                command.Parameters.AddRange(GetDefaultParameters(entity));

                var reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
                int addressId = (int)reader["AddressId"];

                return addressId;
            }
        }

        public Address Read(int entityId)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "SELECT * FROM [Address] " +
                    "WHERE [AddressId] = @AddressId",
                    connection);
                command.Parameters.Add(
                    new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = entityId });

                var reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
                var address = GetAddress(reader);

                return address;
            }
        }

        public bool Update(Address entity)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "UPDATE [Address] " +
                    "SET [CustomerId] = @CustomerId, [AddressLine] = @AddressLine, [AddressLine2] = @AddressLine2, [AddressType] = @AddressType, " +
                    "    [City] = @City, [PostalCode] = @PostalCode, [State] = @State, [Country] = @Country " +
                    "WHERE [AddressId] = @AddressId",
                    connection);
                command.Parameters.Add(
                    new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = entity.AddressId });
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
                    "DELETE FROM [Address] " +
                    "WHERE [AddressId] = @AddressId",
                    connection);
                command.Parameters.Add(
                    new SqlParameter("@AddressId", System.Data.SqlDbType.Int) { Value = entityId });

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
        }

        public int Count()
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "SELECT COUNT(*) FROM [Address]",
                    connection);

                int count = (int)command.ExecuteScalar();

                return count;
            }
        }

        public List<Address> Read(int offset, int count)
        {
            if (count <= 0)
            {
                return new List<Address>();
            }

            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "SELECT * FROM [Address] " +
                    "ORDER BY [AddressId] " +
                    "OFFSET @Offset ROWS " +
                    "FETCH NEXT @Count ROWS ONLY",
                    connection);
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@Offset", System.Data.SqlDbType.Int) { Value = offset },
                    new SqlParameter("@Count", System.Data.SqlDbType.Int) { Value = count }
                });

                var reader = command.ExecuteReader();

                var addresses = new List<Address>();
                while (reader.Read())
                {
                    addresses.Add(GetAddress(reader));
                }

                return addresses;
            }
        }

        public void DeleteAll()
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "DELETE FROM [Address]",
                    connection);

                command.ExecuteNonQuery();
            }
        }
    }
}
