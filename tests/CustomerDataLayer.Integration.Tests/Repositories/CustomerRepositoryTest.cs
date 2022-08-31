using CustomerLibrary.BusinessEntities;
using CustomerLibrary.Repositories;
using FluentAssertions;

namespace CustomerLibrary.Integration.Tests.Repositories
{
    [Collection("CustomerLibraryTests")]
    public class CustomerRepositoryTest
    {
        [Fact]
        public void ShouldBeAbleToCreateCustomerRepository()
        {
            var customerRepository = CustomerRepositoryFixture.GetCustomerRepository();

            customerRepository.Should().NotBeNull();
        }

        [Theory]
        [MemberData(nameof(GenerateCustomers))]
        public void ShouldBeAbleToCreateAndReadCustomer(Customer customer)
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
            customer.CustomerId = customerId;

            var readCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            readCustomer.Should().NotBeNull();
            readCustomer.Should().BeEquivalentTo(customer);
        }

        [Fact]
        public void ShouldNotReadByWrongId()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = CustomerRepositoryFixture.GetMinCustomer();
            CustomerRepositoryFixture.CreateCustomer(customer);

            var readCustomer = CustomerRepositoryFixture.ReadCustomer(0);

            readCustomer.Should().BeNull();
        }

        [Theory]
        [MemberData(nameof(GenerateCustomers))]
        public void ShouldBeAbleToUpdateCustomer(Customer customer)
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;

            var modifiedCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);
            modifiedCustomer.LastName = "New last";
            modifiedCustomer.PhoneNumber = "+12112111111";
            bool isUpdated = CustomerRepositoryFixture.UpdateCustomer(modifiedCustomer);
            var updatedCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            isUpdated.Should().BeTrue();
            updatedCustomer.Should().BeEquivalentTo(modifiedCustomer);
        }

        [Fact]
        public void ShouldNotUpdateByWrongId()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = CustomerRepositoryFixture.GetMinCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
            var createdCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            var modifiedCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);
            modifiedCustomer.CustomerId = 0;
            modifiedCustomer.LastName = "New last";
            modifiedCustomer.PhoneNumber = "+12112111111";
            bool isUpdated = CustomerRepositoryFixture.UpdateCustomer(modifiedCustomer);
            var updatedCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            isUpdated.Should().BeFalse();
            updatedCustomer.Should().BeEquivalentTo(createdCustomer);
        }

        [Fact]
        public void ShouldBeAbleToDeleteCustomer()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = CustomerRepositoryFixture.GetMinCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;

            bool isDeleted = CustomerRepositoryFixture.DeleteCustomer(customerId);
            var deletedCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            isDeleted.Should().BeTrue();
            deletedCustomer.Should().BeNull();
        }

        [Fact]
        public void ShouldNotDeleteByWrongId()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = CustomerRepositoryFixture.GetMinCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
            var createdCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            bool isDeleted = CustomerRepositoryFixture.DeleteCustomer(0);
            var deletedCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            isDeleted.Should().BeFalse();
            deletedCustomer.Should().BeEquivalentTo(createdCustomer);
        }

        [Theory]
        [MemberData(nameof(GenerateDataForCount))]
        public void ShouldBeAbleToCountCustomers(List<Customer> customers)
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            foreach (var customer in customers)
            {
                CustomerRepositoryFixture.CreateCustomer(customer);
            }

            var customersCount = CustomerRepositoryFixture.CountCustomers();

            customersCount.Should().Be(customers.Count);
        }

        [Theory]
        [MemberData(nameof(GenerateDataForReadByOffsetCount))]
        public void ShouldBeAbleToReadByOffsetAndCount(List<Customer> customers, int offset, int count)
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            foreach (var customer in customers)
            {
                int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
                customer.CustomerId = customerId;
            }

            var readCustomers = CustomerRepositoryFixture.ReadCustomers(offset, count);

            readCustomers.Should().NotBeNull();
            readCustomers.Should().BeEquivalentTo(customers.Skip(offset).Take(count));
        }

        [Theory]
        [MemberData(nameof(GenerateDataForDeleteAll))]
        public void ShouldBeAbleToDeleteAllCustomers(List<Customer> customers)
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            foreach (var customer in customers)
            {
                CustomerRepositoryFixture.CreateCustomer(customer);
            }

            CustomerRepositoryFixture.DeleteAllCustomers();

            var deletedCustomers = CustomerRepositoryFixture.ReadCustomers(0, customers.Count);

            deletedCustomers.Should().NotBeNull();
            deletedCustomers.Should().BeEmpty();
        }

        private static IEnumerable<object[]> GenerateCustomers()
        {
            yield return new object[] { CustomerRepositoryFixture.GetMinCustomer() };
            yield return new object[] { CustomerRepositoryFixture.GetMaxCustomer() };
        }

        private static IEnumerable<object[]> GenerateDataForCount()
        {
            yield return new object[] { new List<Customer>(0).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList() };
            yield return new object[] { new List<Customer>(1).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList() };
            yield return new object[] { new List<Customer>(10).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList() };
        }

        private static IEnumerable<object[]> GenerateDataForReadByOffsetCount()
        {
            yield return new object[]
            {
                new List<Customer>(6).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList(),
                2,
                2
            };
            yield return new object[]
            {
                new List<Customer>(6).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList(),
                5,
                3
            };
            yield return new object[]
            {
                new List<Customer>(6).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList(),
                6,
                2
            };
            yield return new object[]
            {
                new List<Customer>(6).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList(),
                2,
                0
            };
            yield return new object[]
            {
                new List<Customer>(6).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList(),
                7,
                1
            };
        }

        private static IEnumerable<object[]> GenerateDataForDeleteAll()
        {
            yield return new object[] { new List<Customer>(0).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList() };
            yield return new object[] { new List<Customer>(1).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList() };
            yield return new object[] { new List<Customer>(10).Select(customer => customer = CustomerRepositoryFixture.GetMinCustomer()).ToList() };
        }
    }

    public static class CustomerRepositoryFixture
    {
        public static Customer GetMinCustomer()
        {
            return new Customer
            {
                LastName = "Last"
            };
        }

        public static Customer GetMaxCustomer()
        {
            return new Customer
            {
                FirstName = "First",
                LastName = "Last",
                PhoneNumber = "+12002000000",
                Email = "a@b.c",
                TotalPurchasesAmount = 10
            };
        }

        public static CustomerRepository GetCustomerRepository()
        {
            return new CustomerRepository();
        }

        public static int? CreateCustomer(Customer customer)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Create(customer);
        }

        public static Customer? ReadCustomer(int customerId)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Read(customerId);
        }

        public static bool UpdateCustomer(Customer customer)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Update(customer);
        }

        public static bool DeleteCustomer(int customerId)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Delete(customerId);
        }

        public static int CountCustomers()
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Count();
        }

        public static List<Customer> ReadCustomers(int offset, int count)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Read(offset, count);
        }

        public static void DeleteAllCustomers()
        {
            var customerRepository = new CustomerRepository();
            customerRepository.DeleteAll();
        }
    }
}
