using CustomerLibrary.Repositories;

namespace CustomerLibrary.Integration.Tests.Repositories
{
    [Collection("CustomerLibraryTests")]
    public class BaseRepositoryTest
    {
        [Fact]
        public void ShouldBeAbleToGetConnection()
        {
            var connection = BaseRepository.GetConnection();

            Assert.NotNull(connection);
        }
    }
}
