using System.Data.SqlClient;

namespace CustomerLibrary.Repositories
{
    public class BaseRepository
    {
        public static SqlConnection GetConnection()
        {
            return new SqlConnection("server=localhost;DataBase=CustomerLib_Tests;Trusted_Connection=True");
        }
    }
}
