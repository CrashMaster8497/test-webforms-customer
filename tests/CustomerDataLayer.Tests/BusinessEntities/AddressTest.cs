using CustomerLibrary.BusinessEntities;

namespace CustomerLibrary.Tests.BusinessEntities
{
    public class AddressTest
    {
        [Fact]
        public void ShouldBeAbleToCreateAddress()
        {
            var address = new Address();

            Assert.NotNull(address);
            Assert.Equal(0, address.AddressId);
            Assert.Equal(0, address.CustomerId);
            Assert.Equal(string.Empty, address.AddressLine);
            Assert.Equal(string.Empty, address.AddressLine2);
            Assert.Equal(AddressType.Unknown, address.AddressType);
            Assert.Equal(string.Empty, address.City);
            Assert.Equal(string.Empty, address.PostalCode);
            Assert.Equal(string.Empty, address.State);
            Assert.Equal(string.Empty, address.Country);
        }
    }
}
