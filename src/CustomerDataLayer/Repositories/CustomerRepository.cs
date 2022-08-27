using CustomerLibrary.BusinessEntities;
using System.Data.SqlClient;

namespace CustomerLibrary.Repositories
{
    public class CustomerRepository : BaseRepository, IRepository<Customer>
    {
        public int? Create(Customer entity)
        {
            using var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "INSERT INTO [Customer] ([FirstName], [LastName], [PhoneNumber], [Email], [TotalPurchasesAmount]) " +
                "OUTPUT INSERTED.[CustomerID] " +
                "VALUES (@FirstName, @LastName, @PhoneNumber, @Email, @TotalPurchasesAmount)",
                connection);
            command.Parameters.AddRange(
                new SqlParameter[]
                {
                    new SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50) { Value = entity.FirstName },
                    new SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50) { Value = entity.LastName },
                    new SqlParameter("@PhoneNumber", System.Data.SqlDbType.VarChar, 12) { Value = entity.PhoneNumber },
                    new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 100) { Value = entity.Email },
                    new SqlParameter("@TotalPurchasesAmount", System.Data.SqlDbType.Money) { Value = entity.TotalPurchasesAmount, IsNullable = true }
                });

            var sqlReader = command.ExecuteReader();

            if (!sqlReader.HasRows)
            {
                return null;
            }

            sqlReader.Read();
            int customerId = (int)sqlReader["CustomerID"];

            return customerId;
        }

        public Customer? Read(int entityId)
        {
            using var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "SELECT * FROM [Customer] " +
                "WHERE [CustomerID] = @CustomerId",
                connection);
            command.Parameters.Add(new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = entityId });

            var sqlReader = command.ExecuteReader();

            if (!sqlReader.HasRows)
            {
                return null;
            }

            sqlReader.Read();
            var customer = new Customer
            {
                CustomerId = (int)sqlReader["CustomerID"],
                FirstName = (string)sqlReader["FirstName"],
                LastName = (string)sqlReader["LastName"],
                PhoneNumber = (string)sqlReader["PhoneNumber"],
                Email = (string)sqlReader["Email"],
                TotalPurchasesAmount = (decimal?)sqlReader["TotalPurchasesAmount"]
            };

            return customer;
        }

        public bool Update(Customer entity)
        {
            using var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "UPDATE [Customer] " +
                "SET [FirstName] = @FirstName, [LastName] = @LastName, [PhoneNumber] = @PhoneNumber, [Email] = @Email, [TotalPurchasesAmount] = @TotalPurchasesAmount " +
                "WHERE [CustomerID] = @CustomerId",
                connection);
            command.Parameters.AddRange(
                new SqlParameter[]
                {
                    new SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50) { Value = entity.FirstName },
                    new SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50) { Value = entity.LastName },
                    new SqlParameter("@PhoneNumber", System.Data.SqlDbType.VarChar, 12) { Value = entity.PhoneNumber },
                    new SqlParameter("@Email", System.Data.SqlDbType.NVarChar, 100) { Value = entity.Email },
                    new SqlParameter("@TotalPurchasesAmount", System.Data.SqlDbType.Money) { Value = entity.TotalPurchasesAmount, IsNullable = true },
                    new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = entity.CustomerId }
                });

            int affectedRows = command.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public bool Delete(int entityId)
        {
            using var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "DELETE FROM [Customer] " +
                "WHERE [CustomerID] = @CustomerId",
                connection);
            command.Parameters.Add(new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = entityId });

            int affectedRows = command.ExecuteNonQuery();

            return affectedRows > 0;
        }

        public void DeleteAll()
        {
            using var connection = GetConnection();
            connection.Open();

            var command = new SqlCommand(
                "DELETE FROM [Note] " +
                "DELETE FROM [Address] " +
                "DELETE FROM [Customer]",
                connection);

            command.ExecuteNonQuery();
        }
    }
}
