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
        public void ShouldNotReadByWrongId()
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
        public void ShouldNotUpdateByWrongId()
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
        public void ShouldNotDeleteByWrongId()
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

        [Theory]
        [MemberData(nameof(GenerateDataForCount))]
        public void ShouldBeAbleToCountAddresses(List<Address> addresses)
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            foreach (var address in addresses)
            {
                AddressRepositoryFixture.CreateAddress(address);
            }

            var addressesCount = AddressRepositoryFixture.CountAddresses();

            addressesCount.Should().Be(addresses.Count);
        }

        [Theory]
        [MemberData(nameof(GenerateDataForReadByOffsetCount))]
        public void ShouldBeAbleToReadByOffsetAndCount(List<Address> addresses, int offset, int count)
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            foreach (var address in addresses)
            {
                int addressId = AddressRepositoryFixture.CreateAddress(address).Value;
                address.AddressId = addressId;
            }

            var readAddresses = AddressRepositoryFixture.ReadAddresses(offset, count);

            readAddresses.Should().NotBeNull();
            readAddresses.Should().BeEquivalentTo(addresses.Skip(offset).Take(count));
        }

        [Theory]
        [MemberData(nameof(GenerateDataForDeleteAll))]
        public void ShouldBeAbleToDeleteAllAddresses(List<Address> addresses)
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            foreach (var address in addresses)
            {
                AddressRepositoryFixture.CreateAddress(address);
            }

            AddressRepositoryFixture.DeleteAllAddresses();

            var deletedAddresses = AddressRepositoryFixture.ReadAddresses(0, addresses.Count);

            deletedAddresses.Should().NotBeNull();
            deletedAddresses.Should().BeEmpty();
        }

        [Fact]
        public void ShouldBeAbleToDeleteByCustomerId()
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            var address = AddressRepositoryFixture.GetDefaultAddress();
            int addressId = AddressRepositoryFixture.CreateAddress(address).Value;

            var readAddress = AddressRepositoryFixture.ReadAddress(addressId);

            int deletedRows = AddressRepositoryFixture.DeleteAddressesByCustomerId(readAddress.CustomerId);
            var deletedAddress = AddressRepositoryFixture.ReadAddress(addressId);

            deletedRows.Should().Be(1);
            deletedAddress.Should().BeNull();
        }

        private static IEnumerable<object[]> GenerateDataForCount()
        {
            List<Address> addresses;

            addresses = new List<Address>(0);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[] { addresses };

            addresses = new List<Address>(1);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[] { addresses };

            addresses = new List<Address>(10);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[] { addresses };
        }

        private static IEnumerable<object[]> GenerateDataForReadByOffsetCount()
        {
            List<Address> addresses;

            addresses = new List<Address>(6);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[]
            {
                addresses,
                2,
                2
            };

            addresses = new List<Address>(6);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[]
            {
                addresses,
                5,
                3
            };

            addresses = new List<Address>(6);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[]
            {
                addresses,
                6,
                2
            };

            addresses = new List<Address>(6);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[]
            {
                addresses,
                2,
                0
            };

            addresses = new List<Address>(6);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[]
            {
                addresses,
                7,
                1
            };
        }

        private static IEnumerable<object[]> GenerateDataForDeleteAll()
        {
            List<Address> addresses;

            addresses = new List<Address>(0);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[] { addresses };

            addresses = new List<Address>(1);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[] { addresses };

            addresses = new List<Address>(10);
            addresses.ForEach(address => address = AddressRepositoryFixture.GetDefaultAddress());
            yield return new object[] { addresses };
        }
    }

    public static class AddressRepositoryFixture
    {
        public static Address GetDefaultAddress()
        {
            return new Address
            {
                AddressLine = "Line",
                AddressLine2 = "Line2",
                AddressType = AddressType.Shipping,
                City = "City",
                PostalCode = "000000",
                State = "State",
                Country = "United States"
            };
        }

        public static AddressRepository GetAddressRepository()
        {
            return new AddressRepository();
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

        public static int CountAddresses()
        {
            var addressRepository = new AddressRepository();
            return addressRepository.Count();
        }

        public static List<Address> ReadAddresses(int offset, int count)
        {
            var addressRepository = new AddressRepository();
            return addressRepository.Read(offset, count);
        }

        public static void DeleteAllAddresses()
        {
            var addressRepository = new AddressRepository();
            addressRepository.DeleteAll();
        }

        public static int DeleteAddressesByCustomerId(int customerId)
        {
            var addressRepository = new AddressRepository();
            return addressRepository.DeleteByCustomerId(customerId);
        }
    }
}
