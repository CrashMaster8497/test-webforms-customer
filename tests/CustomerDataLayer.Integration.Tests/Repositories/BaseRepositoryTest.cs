using CustomerLibrary.Repositories;
using FluentAssertions;

namespace CustomerLibrary.Integration.Tests.Repositories
{
    [Collection("CustomerLibraryTests")]
    public class BaseRepositoryTest
    {
        [Fact]
        public void ShouldBeAbleToGetConnection()
        {
            var connection = BaseRepository.GetConnection();

            connection.Should().NotBeNull();
        }
    }
}
