using CustomerLibrary.BusinessEntities;

namespace CustomerLibrary.Tests.BusinessEntities
{
    public class CustomerTest
    {
        [Fact]
        public void ShouldBeAbleToCreateCustomer()
        {
            var customer = new Customer();

            Assert.NotNull(customer);
            Assert.Equal(0, customer.CustomerId);
            Assert.Equal(string.Empty, customer.FirstName);
            Assert.Equal(string.Empty, customer.LastName);
            Assert.Equal(string.Empty, customer.PhoneNumber);
            Assert.Equal(string.Empty, customer.Email);
            Assert.Null(customer.TotalPurchasesAmount);

            customer.TotalPurchasesAmount = 10;

            Assert.Equal(10m, customer.TotalPurchasesAmount);
        }
    }
}
