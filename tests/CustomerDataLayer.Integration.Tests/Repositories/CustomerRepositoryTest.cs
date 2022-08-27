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

        [Fact]
        public void ShouldBeAbleToReadAllCustomers()
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            List<Customer> customers = new List<Customer>
            {
                CustomerRepositoryFixture.GetDefaultCustomer(),
                CustomerRepositoryFixture.GetDefaultCustomer()
            };
            foreach (Customer customer in customers)
            {
                int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
                customer.CustomerId = customerId;
            }

            var readCustomers = CustomerRepositoryFixture.ReadAllCustomers();

            readCustomers.Should().NotBeNull();
            readCustomers.Should().BeEquivalentTo(customers);
        }

        [Theory]
        [MemberData(nameof(GenerateDataForReadWithOffsetAndCount))]
        public void ShouldBeAbleToReadWithOffsetAndCount(List<Customer> customers, int offset, int count)
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            foreach (Customer customer in customers)
            {
                int customerId = CustomerRepositoryFixture.CreateCustomer(customer).Value;
                customer.CustomerId = customerId;
            }

            var readCustomers = CustomerRepositoryFixture.Read(offset, count);

            readCustomers.Should().NotBeNull();
            readCustomers.Should().BeEquivalentTo(customers.Skip(offset).Take(count));
        }

        [Theory]
        [MemberData(nameof(GenerateDataForCountCustomers))]
        public void ShouldBeAbleToCountCustomers(List<Customer> customers)
        {
            CustomerRepositoryFixture.DeleteAllCustomers();

            foreach (Customer customer in customers)
            {
                CustomerRepositoryFixture.CreateCustomer(customer);
            }

            var customersCount = CustomerRepositoryFixture.CountCustomers();

            customersCount.Should().Be(customers.Count);
        }

        private static IEnumerable<object[]> GenerateDataForReadWithOffsetAndCount()
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
        }

        private static IEnumerable<object[]> GenerateDataForCountCustomers()
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

    public class CustomerRepositoryFixture
    {
        public static CustomerRepository GetCustomerRepository()
        {
            return new CustomerRepository();
        }

        public static Customer GetDefaultCustomer()
        {
            return new Customer
            {
                FirstName = "first",
                LastName = "last",
                PhoneNumber = "+12002000000",
                Email = "a@b.c"
            };
        }

        public static void DeleteAllCustomers()
        {
            var customerRepository = new CustomerRepository();
            customerRepository.DeleteAll();
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

        public static List<Customer> ReadAllCustomers()
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.ReadAll();
        }

        public static List<Customer> Read(int offset, int count)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Read(offset, count);
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
    }
}
