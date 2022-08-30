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

        [Fact]
        public void ShouldBeAbleToCreateAndReadCustomer()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = CustomerRepositoryFixture.GetDefaultCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
            customer.CustomerId = customerId;

            var readCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            readCustomer.Should().NotBeNull();
            readCustomer.Should().BeEquivalentTo(customer);
        }

        [Fact]
        public void ShouldNotReadWithWrongId()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = CustomerRepositoryFixture.GetDefaultCustomer();
            CustomerRepositoryFixture.CreateCustomer(customer);

            var readCustomer = CustomerRepositoryFixture.ReadCustomer(0);

            readCustomer.Should().BeNull();
        }

        [Fact]
        public void ShouldBeAbleToUpdateCustomer()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = CustomerRepositoryFixture.GetDefaultCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;

            var modifiedCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);
            modifiedCustomer.PhoneNumber = "+12112111111";
            bool isUpdated = CustomerRepositoryFixture.UpdateCustomer(modifiedCustomer);
            var updatedCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            isUpdated.Should().BeTrue();
            updatedCustomer.Should().BeEquivalentTo(modifiedCustomer);
        }

        [Fact]
        public void ShouldNotUpdateWithWrongId()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = CustomerRepositoryFixture.GetDefaultCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
            var createdCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            var modifiedCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);
            modifiedCustomer.CustomerId = 0;
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

            var customer = CustomerRepositoryFixture.GetDefaultCustomer();
            int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;

            bool isDeleted = CustomerRepositoryFixture.DeleteCustomer(customerId);
            var deletedCustomer = CustomerRepositoryFixture.ReadCustomer(customerId);

            isDeleted.Should().BeTrue();
            deletedCustomer.Should().BeNull();
        }

        [Fact]
        public void ShouldNotDeleteWithWrongId()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            var customer = CustomerRepositoryFixture.GetDefaultCustomer();
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
        [MemberData(nameof(GenerateDataForReadWithOffsetCount))]
        public void ShouldBeAbleToReadWithOffsetAndCount(List<Customer> customers, int offset, int count)
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

        private static IEnumerable<object[]> GenerateDataForCount()
        {
            List<Customer> customers;

            customers = new List<Customer>(0);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[] { customers };

            customers = new List<Customer>(1);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[] { customers };

            customers = new List<Customer>(10);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[] { customers };
        }

        private static IEnumerable<object[]> GenerateDataForReadWithOffsetCount()
        {
            List<Customer> customers;

            customers = new List<Customer>(6);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[]
            {
                customers,
                2,
                2
            };

            customers = new List<Customer>(6);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[]
            {
                customers,
                5,
                3
            };

            customers = new List<Customer>(6);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[]
            {
                customers,
                6,
                2
            };

            customers = new List<Customer>(6);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[]
            {
                customers,
                2,
                0
            };

            customers = new List<Customer>(6);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[]
            {
                customers,
                7,
                1
            };
        }

        private static IEnumerable<object[]> GenerateDataForDeleteAll()
        {
            List<Customer> customers;

            customers = new List<Customer>(0);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[] { customers };

            customers = new List<Customer>(1);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[] { customers };

            customers = new List<Customer>(10);
            customers.ForEach(customer => customer = CustomerRepositoryFixture.GetDefaultCustomer());
            yield return new object[] { customers };
        }
    }

    public static class CustomerRepositoryFixture
    {
        public static Customer GetDefaultCustomer()
        {
            return new Customer
            {
                FirstName = "First",
                LastName = "Last",
                PhoneNumber = "+12002000000",
                Email = "a@b.c"
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
