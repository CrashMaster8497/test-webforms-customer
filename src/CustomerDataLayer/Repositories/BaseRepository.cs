using System.Data.SqlClient;

namespace CustomerLibrary.Repositories
{
    public abstract class BaseRepository
    {
        public static SqlConnection GetConnection()
        {
            var connection = new SqlConnection("server=localhost;DataBase=CustomerLib_Tests;Trusted_Connection=True");
            connection.Open();
            return connection;
        }
    }
}
