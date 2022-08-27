using CustomerLibrary.BusinessEntities;
using CustomerLibrary.Repositories;

namespace CustomerLibrary.Integration.Tests.Repositories
{
    [Collection("CustomerLibraryTests")]
    public class IRepositoryTest
    {
        [Theory]
        [MemberData(nameof(RepositoryTestData))]
        public void ShouldBeAbleToCreateRepository(object repository, Type interfaceType)
        {
            Assert.Contains(interfaceType, repository.GetType().GetInterfaces());
            Assert.NotNull(repository);
        }

        private static IEnumerable<object[]> RepositoryTestData()
        {
            yield return new object[] { new CustomerRepository(), typeof(IRepository<Customer>) };
            yield return new object[] { new AddressRepository(), typeof(IRepository<Address>) };
            yield return new object[] { new NoteRepository(), typeof(IRepository<Note>) };
        }
    }
}
