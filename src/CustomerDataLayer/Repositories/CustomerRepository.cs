using CustomerLibrary.BusinessEntities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CustomerLibrary.Repositories
{
    public class CustomerRepository : BaseRepository, IRepository<Customer>
    {
        public int? Create(Customer entity)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "INSERT INTO [Customer] ([FirstName], [LastName], [PhoneNumber], [Email], [TotalPurchasesAmount]) " +
                    "OUTPUT INSERTED.[CustomerId] " +
                    "VALUES (@FirstName, @LastName, @PhoneNumber, @Email, @TotalPurchasesAmount)",
                    connection);
                command.Parameters.AddRange(GetDefaultParameters(entity));

                var reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
                int customerId = (int)reader["CustomerId"];

                return customerId;
            }
        }

        public Customer Read(int entityId)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "SELECT * FROM [Customer] " +
                    "WHERE [CustomerId] = @CustomerId",
                    connection);
                command.Parameters.Add(
                    new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = entityId });

                var reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    return null;
                }

                reader.Read();
                var customer = GetCustomer(reader);

                return customer;
            }
        }

        public bool Update(Customer entity)
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "UPDATE [Customer] " +
                    "SET [FirstName] = @FirstName, [LastName] = @LastName, [PhoneNumber] = @PhoneNumber, [Email] = @Email, [TotalPurchasesAmount] = @TotalPurchasesAmount " +
                    "WHERE [CustomerId] = @CustomerId",
                    connection);
                command.Parameters.Add(
                    new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = entity.CustomerId });
                command.Parameters.AddRange(GetDefaultParameters(entity));

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
        }

        public bool Delete(int entityId)
        {
            var addressRepository = new AddressRepository();
            addressRepository.DeleteByCustomerId(entityId);

            var noteRepository = new NoteRepository();
            noteRepository.DeleteByCustomerId(entityId);

            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "DELETE FROM [Customer] " +
                    "WHERE [CustomerId] = @CustomerId",
                    connection);
                command.Parameters.Add(
                    new SqlParameter("@CustomerId", System.Data.SqlDbType.Int) { Value = entityId });

                int affectedRows = command.ExecuteNonQuery();

                return affectedRows > 0;
            }
        }

        public int Count()
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "SELECT COUNT(*) FROM [Customer]",
                    connection);

                int count = (int)command.ExecuteScalar();

                return count;
            }
        }

        public List<Customer> Read(int offset, int count)
        {
            if (count <= 0)
            {
                return new List<Customer>();
            }

            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "SELECT * FROM [Customer] " +
                    "ORDER BY [CustomerId] " +
                    "OFFSET @Offset ROWS " +
                    "FETCH NEXT @Count ROWS ONLY",
                    connection);
                command.Parameters.AddRange(new SqlParameter[]
                {
                    new SqlParameter("@Offset", System.Data.SqlDbType.Int) { Value = offset },
                    new SqlParameter("@Count", System.Data.SqlDbType.Int) { Value = count }
                });

                var reader = command.ExecuteReader();

                var customers = new List<Customer>();
                while (reader.Read())
                {
                    customers.Add(GetCustomer(reader));
                }

                return customers;
            }
        }

        public void DeleteAll()
        {
            using (var connection = GetConnection())
            {
                var command = new SqlCommand(
                    "DELETE FROM [Address] " +
                    "DELETE FROM [Note] " +
                    "DELETE FROM [Customer]",
                    connection);

                command.ExecuteNonQuery();
            }
        }

        private static SqlParameter[] GetDefaultParameters(Customer customer)
        {
            return new SqlParameter[]
            {
                new SqlParameter("@FirstName", System.Data.SqlDbType.NVarChar, 50)
                {
                    Value = string.IsNullOrWhiteSpace(customer.FirstName) ? DBNull.Value : (object)customer.FirstName,
                    IsNullable = true
                },
                new SqlParameter("@LastName", System.Data.SqlDbType.NVarChar, 50)
                {
                    Value = customer.LastName
                },
                new SqlParameter("@PhoneNumber", System.Data.SqlDbType.VarChar, 12)
                {
                    Value = string.IsNullOrWhiteSpace(customer.PhoneNumber) ? DBNull.Value : (object)customer.PhoneNumber,
                    IsNullable = true
                },
                new SqlParameter("@Email", System.Data.SqlDbType.VarChar, 100)
                {
                    Value = string.IsNullOrWhiteSpace(customer.Email) ? DBNull.Value : (object)customer.Email,
                    IsNullable = true
                },
                new SqlParameter("@TotalPurchasesAmount", System.Data.SqlDbType.Money)
                {
                    Value = (object)customer.TotalPurchasesAmount ?? DBNull.Value,
                    IsNullable = true
                }
            };
        }

        private static Customer GetCustomer(SqlDataReader reader)
        {
            return new Customer
            {
                CustomerId = (int)reader["CustomerId"],
                FirstName = reader["FirstName"] == DBNull.Value ? string.Empty : (string)reader["FirstName"],
                LastName = (string)reader["LastName"],
                PhoneNumber = reader["PhoneNumber"] == DBNull.Value ? string.Empty : (string)reader["PhoneNumber"],
                Email = reader["Email"] == DBNull.Value ? string.Empty : (string)reader["Email"],
                TotalPurchasesAmount = reader["TotalPurchasesAmount"] == DBNull.Value ? null : (decimal?)reader["TotalPurchasesAmount"]
            };
        }
    }
}
