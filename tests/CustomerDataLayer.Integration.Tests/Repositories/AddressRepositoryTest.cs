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

        [Theory]
        [MemberData(nameof(GenerateAddresses))]
        public void ShouldBeAbleToCreateAndReadAddress(Address address)
        {
            AddressRepositoryFixture.DeleteAllAddresses();

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

            var address = AddressRepositoryFixture.GetMinAddress();
            AddressRepositoryFixture.CreateAddress(address);

            var readAddress = AddressRepositoryFixture.ReadAddress(0);

            readAddress.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(GenerateAddresses))]
        public void ShouldBeAbleToUpdateAddress(Address address)
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            int addressId = AddressRepositoryFixture.CreateAddress(address).Value;

            var modifiedAddress = AddressRepositoryFixture.ReadAddress(addressId);
            modifiedAddress.AddressLine2 = "New line2";
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

            var address = AddressRepositoryFixture.GetMinAddress();
            int addressId = AddressRepositoryFixture.CreateAddress(address).Value;
            var createdAddress = AddressRepositoryFixture.ReadAddress(addressId);

            var modifiedAddress = AddressRepositoryFixture.ReadAddress(addressId);
            modifiedAddress.AddressId = 0;
            modifiedAddress.AddressLine2 = "New line2";
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

            var address = AddressRepositoryFixture.GetMinAddress();
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

            var address = AddressRepositoryFixture.GetMinAddress();
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

        [Theory]
        [MemberData(nameof(GenerateDataForReadByCustomerId))]
        public void ShouldBeAbleToReadByCustomerId(List<List<Address>> addresses, List<Customer> customers, List<int> offsets, List<int> counts)
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            for (int i = 0; i < customers.Count; i++)
            {
                int customerId = CustomerRepositoryFixture.CreateCustomer(customers[i]).Value;
                customers[i].CustomerId = customerId;

                foreach (var address in addresses[i])
                {
                    int addressId = AddressRepositoryFixture.CreateAddress(address, customers[i]).Value;
                    address.AddressId = addressId;
                }
            }

            for (int i = 0; i < customers.Count; i++)
            {
                var readAddresses = AddressRepositoryFixture.ReadAddressesByCustomerId(customers[i].CustomerId, offsets[i], counts[i]);

                readAddresses.Should().BeEquivalentTo(addresses[i].Skip(offsets[i]).Take(counts[i]));
            }
        }

        [Theory]
        [MemberData(nameof(GenerateDataForDeleteByCustomerId))]
        public void ShouldBeAbleToDeleteByCustomerId(List<List<Address>> addresses, List<Customer> customers)
        {
            AddressRepositoryFixture.DeleteAllAddresses();

            for (int i = 0; i < customers.Count; i++)
            {
                int customerId = CustomerRepositoryFixture.CreateCustomer(customers[i]).Value;
                customers[i].CustomerId = customerId;

                foreach (var address in addresses[i])
                {
                    AddressRepositoryFixture.CreateAddress(address, customers[i]);
                }
            }

            for (int i = 0; i < customers.Count; i++)
            {
                int deletedRows = AddressRepositoryFixture.DeleteAddressesByCustomerId(customers[i].CustomerId);
                var deletedAddresses = AddressRepositoryFixture.ReadAddressesByCustomerId(customers[i].CustomerId, 0, addresses[i].Count);

                deletedRows.Should().Be(addresses[i].Count);
                deletedAddresses.Should().BeEmpty();

                for (int j = i + 1; j < customers.Count; i++)
                {
                    var notDeletedAddresses = AddressRepositoryFixture.ReadAddressesByCustomerId(customers[i].CustomerId, 0, addresses[i].Count);

                    notDeletedAddresses.Should().NotBeEmpty();
                }
            }
        }

        private static IEnumerable<object[]> GenerateAddresses()
        {
            yield return new object[] { AddressRepositoryFixture.GetMinAddress() };
            yield return new object[] { AddressRepositoryFixture.GetMaxAddress() };
        }

        private static IEnumerable<object[]> GenerateDataForCount()
        {
            yield return new object[] { new List<Address>(0).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList() };
            yield return new object[] { new List<Address>(1).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList() };
            yield return new object[] { new List<Address>(10).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList() };
        }

        private static IEnumerable<object[]> GenerateDataForReadByOffsetCount()
        {
            yield return new object[]
            {
                new List<Address>(6).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                2,
                2
            };
            yield return new object[]
            {
                new List<Address>(6).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                5,
                3
            };
            yield return new object[]
            {
                new List<Address>(6).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                6,
                2
            };
            yield return new object[]
            {
                new List<Address>(6).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                2,
                0
            };
            yield return new object[]
            {
                new List<Address>(6).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                7,
                1
            };
        }

        private static IEnumerable<object[]> GenerateDataForDeleteAll()
        {
            yield return new object[] { new List<Address>(0).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList() };
            yield return new object[] { new List<Address>(1).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList() };
            yield return new object[] { new List<Address>(10).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList() };
        }

        private static IEnumerable<object[]> GenerateDataForReadByCustomerId()
        {
            yield return new object[]
            {
                new List<List<Address>>
                {
                    new List<Address>(1).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                    new List<Address>(0).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                    new List<Address>(4).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList()
                },
                new List<Customer>(3).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList(),
                new List<int> { 0, 0, 0 },
                new List<int> { 1, 0, 4 }
            };
            yield return new object[]
            {
                new List<List<Address>>
                {
                    new List<Address>(1).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                    new List<Address>(0).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                    new List<Address>(4).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList()
                },
                new List<Customer>(3).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList(),
                new List<int> { 1, 1, 2 },
                new List<int> { 0, 2, 4 }
            };
        }

        private static IEnumerable<object[]> GenerateDataForDeleteByCustomerId()
        {
            yield return new object[]
            {
                new List<List<Address>>
                {
                    new List<Address>(1).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                    new List<Address>(0).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList(),
                    new List<Address>(3).Select(address => address = AddressRepositoryFixture.GetMinAddress()).ToList()
                },
                new List<Customer>(3).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList()
            };
        }
    }

    public static class AddressRepositoryFixture
    {
        public static Address GetMinAddress()
        {
            return new Address
            {
                AddressLine = "Line",
                AddressType = AddressType.Shipping,
                City = "City",
                PostalCode = "000000",
                State = "State",
                Country = "United States"
            };
        }

        public static Address GetMaxAddress()
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
            var customer = CustomerRepositoryFixture.GetMinCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
            address.CustomerId = customerId;

            var addressRepository = new AddressRepository();
            return addressRepository.Create(address);
        }

        public static int? CreateAddress(Address address, Customer customer)
        {
            var addressRepository = new AddressRepository();
            address.CustomerId = customer.CustomerId;
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

        public static List<Address> ReadAddressesByCustomerId(int customerId, int offset, int count)
        {
            var addressRepository = new AddressRepository();
            return addressRepository.ReadByCustomerId(customerId, offset, count);
        }

        public static int DeleteAddressesByCustomerId(int customerId)
        {
            var addressRepository = new AddressRepository();
            return addressRepository.DeleteByCustomerId(customerId);
        }
    }
}
