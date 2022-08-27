using CustomerLibrary.BusinessEntities;
using CustomerLibrary.Repositories;
using FluentAssertions;

namespace CustomerLibrary.Integration.Tests.Repositories
{
    [Collection("CustomerLibraryTests")]
    public class AddressRepositoryTest
    {
        [Fact]
        public void ShouldBeAbleToCreateAddressRepository()
        {
            var addressRepository = AddressRepositoryFixture.GetAddressRepository();

            addressRepository.Should().NotBeNull();
        }

        [Fact]
        public void ShouldBeAbleToCreateAndReadAddress()
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            var address = AddressRepositoryFixture.GetDefaultAddress();
            int addressId = AddressRepositoryFixture.CreateAddress(address).Value;
            address.AddressId = addressId;

            var readAddress = AddressRepositoryFixture.ReadAddress(addressId);

            readAddress.Should().NotBeNull();
            readAddress.Should().BeEquivalentTo(address);
        }

        [Fact]
        public void ShouldNotReadWithWrongId()
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            var address = AddressRepositoryFixture.GetDefaultAddress();
            AddressRepositoryFixture.CreateAddress(address);

            var readAddress = AddressRepositoryFixture.ReadAddress(0);

            readAddress.Should().BeNull();
        }

        [Fact]
        public void ShouldBeAbleToUpdateAddress()
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            var address = AddressRepositoryFixture.GetDefaultAddress();
            int addressId = AddressRepositoryFixture.CreateAddress(address).Value;

            var modifiedAddress = AddressRepositoryFixture.ReadAddress(addressId);
            modifiedAddress.PostalCode = "012345";
            bool isUpdated = AddressRepositoryFixture.UpdateAddress(modifiedAddress);
            var updatedAddress = AddressRepositoryFixture.ReadAddress(addressId);

            isUpdated.Should().BeTrue();
            updatedAddress.Should().BeEquivalentTo(modifiedAddress);
        }

        [Fact]
        public void ShouldNotUpdateWithWrongId()
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            var address = AddressRepositoryFixture.GetDefaultAddress();
            int addressId = AddressRepositoryFixture.CreateAddress(address).Value;
            var createdAddress = AddressRepositoryFixture.ReadAddress(addressId);

            var modifiedAddress = AddressRepositoryFixture.ReadAddress(addressId);
            modifiedAddress.AddressId = 0;
            modifiedAddress.PostalCode = "012345";
            bool isUpdated = AddressRepositoryFixture.UpdateAddress(modifiedAddress);
            var updatedAddress = AddressRepositoryFixture.ReadAddress(addressId);

            isUpdated.Should().BeFalse();
            updatedAddress.Should().BeEquivalentTo(createdAddress);
        }

        [Fact]
        public void ShouldBeAbleToDeleteAddress()
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            var address = AddressRepositoryFixture.GetDefaultAddress();
            int addressId = AddressRepositoryFixture.CreateAddress(address).Value;

            bool isDeleted = AddressRepositoryFixture.DeleteAddress(addressId);
            var deletedAddress = AddressRepositoryFixture.ReadAddress(addressId);

            isDeleted.Should().BeTrue();
            deletedAddress.Should().BeNull();
        }

        [Fact]
        public void ShouldNotDeleteWithWrongId()
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            var address = AddressRepositoryFixture.GetDefaultAddress();
            int addressId = AddressRepositoryFixture.CreateAddress(address).Value;
            var createdAddress = AddressRepositoryFixture.ReadAddress(addressId);

            bool isDeleted = AddressRepositoryFixture.DeleteAddress(0);
            var deletedAddress = AddressRepositoryFixture.ReadAddress(addressId);

            isDeleted.Should().BeFalse();
            deletedAddress.Should().BeEquivalentTo(createdAddress);
        }
    }

    public class AddressRepositoryFixture
    {
        public static AddressRepository GetAddressRepository()
        {
            return new AddressRepository();
        }

        public static Address GetDefaultAddress()
        {
            return new Address
            {
                AddressLine = "line",
                AddressLine2 = "line2",
                AddressType = AddressType.Shipping,
                City = "city",
                PostalCode = "000000",
                State = "state",
                Country = "United States"
            };
        }

        public static void DeleteAllAddresses()
        {
            var addressRepository = new AddressRepository();
            addressRepository.DeleteAll();
        }

        public static int? CreateAddress(Address address)
        {
            var customer = CustomerRepositoryFixture.GetDefaultCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
            address.CustomerId = customerId;

            var addressRepository = new AddressRepository();
            return addressRepository.Create(address);
        }

        public static Address? ReadAddress(int addressId)
        {
            var addressRepository = new AddressRepository();
            return addressRepository.Read(addressId);
        }

        public static bool UpdateAddress(Address address)
        {
            var addressRepository = new AddressRepository();
            return addressRepository.Update(address);
        }

        public static bool DeleteAddress(int addressId)
        {
            var addressRepository = new AddressRepository();
            return addressRepository.Delete(addressId);
        }
    }
}
