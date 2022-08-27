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
                Email = "a@b.c",
                TotalPurchasesAmount = 0
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

        public static bool DeleteCustomer(int customerId)
        {
            var customerRepository = new CustomerRepository();
            return customerRepository.Delete(customerId);
        }
    }
}
